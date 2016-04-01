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
    public partial class ImportSpeciesAndStates : System.Web.UI.Page
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
                    // remove existing species and their character states
                    foreach (var sp in context.Species)
                    {
                        var states = sp.CharacterStates.ToArray();
                        foreach (var state in states)
                        {
                            sp.CharacterStates.Remove(state);
                        }

                        context.Species.Remove(sp);
                    }

                    context.SaveChanges();
                }

                string excelSpeciesColName = ExcelUtils.GetExcelColumnName(FriendlyColumnNames.SpeciesName, columnMapping);

                foreach (DataRow dataRow in dt.Rows)
                {
                    string speciesName = ExcelUtils.GetExcelRowValueForColumn(dataRow, excelSpeciesColName).ToString().Trim();

                    Species sp = context.Species.SingleOrDefault(i => i.SpeciesName == speciesName);
                    if (sp == null)
                    {
                        sp = context.Species.Create();
                        sp.AssignNewSpeciesId(context);
                        sp.SpeciesName = speciesName;
                        context.Species.Add(sp);
                    }

                    foreach (DataColumn col in dataRow.Table.Columns)
                    {
                        string colName = col.ColumnName;
                        if (colName != columnMapping[FriendlyColumnNames.SpeciesName.ToUpper()]) //not species column
                        {
                            Character cha = context.Characters.SingleOrDefault(i => i.CharacterCode == colName);
                            if (cha == null)
                            {
                                //excel contains non character code columns -- Error
                                throw new Exception(colName + " is not a valid Character Code. Please correct this and re-import.");
                            }
                            else
                            {
                                string stateCode = dataRow[colName].ToString().Trim();

                                if (!string.IsNullOrEmpty(stateCode) && stateCode != "-")
                                {
                                    CharacterState st = context.CharacterStates.SingleOrDefault(i => i.CharacterID == cha.CharacterID && i.CharacterStateCode == stateCode);
                                    if (st == null)
                                    {
                                        string errMsg = string.Format("{0} is not a valid Character State for the character: {1} for {2}. Please correct this and re-import.", stateCode, cha.CharacterCode, speciesName);
                                        throw new Exception(errMsg);
                                    }
                                    else
                                    {
                                        sp.CharacterStates.Add(st);
                                    }
                                }
                            }
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
            string[] dbCols = FriendlyColumnNames.GetAllSpeciesColumnNames();

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

            using (var spreadSheetDocument = new ExcelPackage(stream))
            {
                colNames = ExcelUtils.GetExcelHeaderNames(spreadSheetDocument).Take(3).ToList();
            }

            dgvColumns.DataSource = colNames;
            dgvColumns.DataBind();
        }



        protected void CustomValidatorValidateMapping_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string currentCol = string.Empty;
            Dictionary<string, string> columnMapping = new Dictionary<string, string>();

            try
            {

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
                    lblNoError.Text = "No errors found. Click Finish to Import.";
                }
            }
            catch (Exception ex)
            {
                CustomValidatorValidateMapping.ErrorMessage = string.Format("{0} mapped twice!", currentCol);
                lblNoError.Text = "";
                args.IsValid = false;
            }

            MemoryStream stream = new MemoryStream(ViewState["Stream"] as byte[]);
            DataTable dt = new DataTable();

            using (ExcelPackage spreadSheetDocument = new ExcelPackage(stream))
            {
                try
                {
                    dt = ExcelUtils.GetDataTableFromExcel(spreadSheetDocument, true);
                }
                catch (Exception ex)
                {
                    CustomValidatorValidateMapping.ErrorMessage = ex.Message;
                }
            }

            string speciesColName = ExcelUtils.GetExcelColumnName(FriendlyColumnNames.SpeciesName, columnMapping);

            using (InteractiveKeyEntities context = new InteractiveKeyEntities())
            {
                foreach (DataColumn dtColumn in dt.Columns)
                {
                    if (!dtColumn.ColumnName.Equals(speciesColName))
                    {
                        var colName = dtColumn.ColumnName;
                        Character cha = context.Characters.SingleOrDefault(i => i.CharacterCode == colName);
                        if (cha == null)
                        {
                            CustomValidatorValidateMapping.ErrorMessage = string.Format("Cannot find Character Code: {0} in the database!", colName);
                            lblNoError.Text = "";
                            args.IsValid = false;
                            break;
                        }

                        foreach (DataRow dtRow in dt.Rows)
                        {
                            string stateCode = Convert.ToString(ExcelUtils.GetExcelRowValueForColumn(dtRow, colName));
                            if (!string.IsNullOrEmpty(stateCode))
                            {
                                bool validStateCode = cha.CharacterStates.Any(i => i.CharacterStateCode == stateCode);
                                if (!validStateCode)
                                {
                                    CustomValidatorValidateMapping.ErrorMessage = string.Format("Cannot find Character State Code: {0} under Character: {1} in the database!", stateCode, colName);
                                    lblNoError.Text = "";
                                    args.IsValid = false;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
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