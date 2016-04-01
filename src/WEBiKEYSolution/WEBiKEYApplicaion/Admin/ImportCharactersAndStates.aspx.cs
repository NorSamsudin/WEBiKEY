using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using OfficeOpenXml;
using WEBiKEY.Application.Classes;

namespace WEBiKEY.Application.Admin
{
    public partial class ImportCharactersAndStates : System.Web.UI.Page
    {
        public Dictionary<string, string> items;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ExcelImportWizard_FinishButtonClick(object sender, WizardNavigationEventArgs e)
        {
            if (!CustomValidatorValidateMapping.IsValid)
            {
                return;
            }

            string appendMode = ViewState["AppendMode"].ToString();

            Dictionary<string, string> columnMapping = new Dictionary<string, string>();

            foreach (GridViewRow row in dgvColumns.Rows)
            {
                string excelColumn = row.Cells[0].Text;
                string dbColumn = (row.Cells[1].Controls[1] as DropDownList).SelectedValue;

                if (dbColumn.Equals(ExcelUtils.IGNORE_COLUMN_VALUE, StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }

                columnMapping.Add(dbColumn, excelColumn);
            }

            MemoryStream stream = new MemoryStream(ViewState["Stream"] as byte[]);
            DataTable dt;

            using (ExcelPackage spreadSheetDocument = new ExcelPackage(stream))
            {
                dt = ExcelUtils.GetDataTableFromExcel(spreadSheetDocument, true);
            }


            using (InteractiveKeyEntities context = new InteractiveKeyEntities())
            {
                if (appendMode == "Overwrite")
                {
                    foreach (var sp in context.Species)
                    {
                        sp.CharacterStates.Clear();

                        context.Species.Remove(sp);
                    }

                    foreach (var cs in context.CharacterStates)
                    {
                        cs.DisabledCharacters.Clear();
                    }

                    foreach (var st in context.CharacterStates)
                    {
                        context.CharacterStates.Remove(st);
                    }

                    foreach (var ch in context.Characters)
                    {
                        context.Characters.Remove(ch);
                    }

                    foreach (var cat in context.CharacterCategories)
                    {
                        context.CharacterCategories.Remove(cat);
                    }

                    context.SaveChanges();
                }

                foreach (DataRow dataRow in dt.Rows)
                {
                    string categoryName = ExcelUtils.GetExcelRowValueForColumn(dataRow, FriendlyColumnNames.CategoryName, columnMapping).ToString();
                    string characterCode = ExcelUtils.GetExcelRowValueForColumn(dataRow, FriendlyColumnNames.CharacterCode, columnMapping).ToString();
                    string characterDesc = ExcelUtils.GetExcelRowValueForColumn(dataRow, FriendlyColumnNames.CharacterDescription, columnMapping).ToString();
                    string stateCode = ExcelUtils.GetExcelRowValueForColumn(dataRow, FriendlyColumnNames.CharacterStateCode, columnMapping).ToString();
                    string stateDesc = ExcelUtils.GetExcelRowValueForColumn(dataRow, FriendlyColumnNames.CharacterStateDescription, columnMapping).ToString();

                    CharacterCategory cat = context.CharacterCategories.SingleOrDefault(i => i.CategoryName == categoryName);
                    if (cat == null)
                    {
                        cat = context.CharacterCategories.Create();
                        cat.AssignNewdCharacterCategoryId(context);
                        cat.CategoryName = categoryName;
                        context.CharacterCategories.Add(cat);
                    }

                    Character cha = context.Characters.SingleOrDefault(i => i.CharacterCode == characterCode);
                    if (cha == null)
                    {
                        cha = context.Characters.Create();
                        cha.AssignNewdCharacterId(context);
                        cha.CharacterCode = characterCode;
                        cha.CharacterDescription = characterDesc;
                        cat.Characters.Add(cha);

                        CharacterState st = context.CharacterStates.Create();
                        st.AssignNewCharacterStateId(context);
                        st.CharacterStateCode = stateCode;
                        st.CharacterStateDescription = stateDesc;
                        cha.CharacterStates.Add(st);
                    }
                    else
                    {
                        CharacterState st = context.CharacterStates.SingleOrDefault(i => i.CharacterStateCode == characterCode);
                        if (st == null)
                        {
                            st = context.CharacterStates.Create();
                            st.AssignNewCharacterStateId(context);
                            st.CharacterStateCode = stateCode;
                            st.CharacterStateDescription = stateDesc;
                            cha.CharacterStates.Add(st);
                        }
                    }

                    context.SaveChanges();
                }

                context.SaveChanges();
            }
        }

        protected void ExcelImportWizard_NextButtonClick(object sender, WizardNavigationEventArgs e)
        {
            if (e.CurrentStepIndex == 0)
            {
                if (fileUploadExcel.HasFile)
                {
                    lblInfo.Visible = false;
                    lblExcelFileInfo.Text = "";

                    string FileName = Path.GetFileName(fileUploadExcel.PostedFile.FileName);
                    string Extension = Path.GetExtension(fileUploadExcel.PostedFile.FileName);

                    if (!Extension.Equals(".xlsx", StringComparison.InvariantCultureIgnoreCase))
                    {
                        lblExcelFileInfo.Text = "<br />Excel File Format Must be 2007 or Newer";
                        e.Cancel = true;
                        return;
                    }
                    //string FolderPath = ConfigurationManager.AppSettings["FolderPath"];

                    //string FilePath = Server.MapPath(FolderPath + FileName);
                    //fileUploadExcel.SaveAs(FilePath);

                    ViewState["Stream"] = fileUploadExcel.FileBytes;
                    MemoryStream ms = new MemoryStream(ViewState["Stream"] as byte[]);

                    //ViewState["ExcelFilePath"] = FilePath;
                    ViewState["ExcelFileExtension"] = Extension;

                    lblInfo.Text = "File uploaded";

                    items = new Dictionary<string, string>();
                    items.Add(ExcelUtils.IGNORE_COLUMN_VALUE.ToUpper(), ExcelUtils.IGNORE_COLUMN_VALUE);

                    string[] dbFields = getDbFields();
                    foreach (string item in dbFields)
                    {
                        items.Add(item.ToUpper(), item);
                    }


                    string appendMode = RadioButtonListOptions.SelectedValue;

                    ViewState["AppendMode"] = appendMode;

                    ImportToGrid(ms);
                    MatchExcelAndDbColumns();
                }
                else
                {
                    lblInfo.Text = "No file selected!";
                }
            }
        }

        protected void ExcelImportWizard_PreviousButtonClick(object sender, WizardNavigationEventArgs e)
        {

        }

        private void MatchExcelAndDbColumns()
        {
            foreach (GridViewRow row in dgvColumns.Rows)
            {
                string excelCol = row.Cells[0].Text.Trim().ToUpper();
                IQueryable<ListItem> dbColItems = (row.Cells[1].FindControl("ItemDropDown") as DropDownList).Items.Cast<ListItem>().AsQueryable() as IQueryable<ListItem>;
                string[] dbCols = dbColItems.Where(i => !i.Value.Contains("IGNORE")).Select(i => i.Value).ToArray();

                string bestMatch = LevenshteinDistance.FindBestMatch(excelCol, dbCols);
                ListItem bestItem = (row.Cells[1].FindControl("ItemDropDown") as DropDownList).Items.FindByValue(bestMatch);

                if (bestItem != null)
                {
                    bestItem.Selected = true;
                }
            }
        }

        private string[] getDbFields()
        {
            string[] dbCols = FriendlyColumnNames.GetAllCharacterColumnNames();

            return dbCols;
        }

        protected void ExcelImportWizard_ActiveStepChanged(object sender, EventArgs e)
        {
            if (ExcelImportWizard.ActiveStepIndex == 1)
            {
                CustomValidatorValidateMapping.Validate();
            }
        }

        private void ImportToGrid(Stream stream)
        {
            List<string> colNames;

            using (ExcelPackage spreadSheetDocument = new ExcelPackage(stream))
            {
                colNames = ExcelUtils.GetExcelHeaderNames(spreadSheetDocument).ToList();
            }

            dgvColumns.DataSource = colNames;
            dgvColumns.DataBind();
        }

        protected void CustomValidatorValidateMapping_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string currentCol = string.Empty;
            try
            {
                Dictionary<string, string> columnMapping = new Dictionary<string, string>();

                foreach (GridViewRow row in dgvColumns.Rows)
                {
                    string excelColumn = row.Cells[0].Text;
                    string dbColumn = (row.Cells[1].Controls[1] as DropDownList).SelectedValue;

                    if (dbColumn.Equals(ExcelUtils.IGNORE_COLUMN_VALUE, StringComparison.CurrentCultureIgnoreCase))
                    {
                        continue;
                    }

                    currentCol = dbColumn;
                    columnMapping.Add(dbColumn, excelColumn);
                }
            }
            catch (Exception ex)
            {
                CustomValidatorValidateMapping.ErrorMessage = string.Format("{0} mapped twice!", currentCol);
                args.IsValid = false;
            }

            //MemoryStream stream = new MemoryStream(ViewState["Stream"] as byte[]);
            //DataTable dt = new DataTable();

            //using (ExcelPackage spreadSheetDocument = new ExcelPackage(stream))
            //{
            //    dt = ExcelUtils.GetDataTableFromExcel(spreadSheetDocument, true);
            //}
        }

        protected void ItemDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            CustomValidatorValidateMapping.Validate();
        }

        protected void btnValidate_Click(object sender, EventArgs e)
        {
            CustomValidatorValidateMapping.Validate();
        }
    }
}