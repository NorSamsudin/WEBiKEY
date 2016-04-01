<%@ Page Title="" Language="C#" MasterPageFile="~/Master/SiteMaster.Master" AutoEventWireup="true" CodeBehind="Help.aspx.cs" Inherits="WEBiKEY.Application.Help" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderContent" runat="server">

    <div class="margin-10">
        <h3>How to use the <em>Kuruna</em> WEBKEY interactive key</h3>
        <p>
            This web-based application has three main web pages: a home page (Fig. 1), a major character group selection page (Fig. 2) and a detailed interactive key page (Fig. 3), along with
            three menus that allow users to download each <em>Kuruna</em> species description, a dichotomous key or a glossary in PDF form.  
        </p>
        <div id="help-content">
            <ol>
                <li>
                    <strong>Home Page</strong>
                    <p>
                        The home page of the <em>Kuruna</em> interactive key gives a brief description about bamboos in general and about the genus <em>Kuruna</em>. Fifty five different characters were used
                        in an interactive
                        key distinguishing the seven <em>Kuruna</em> species in Sri Lanka and south India.

                    </p>
                    <img src="Images/Fig1.jpg" />
                    <p class="figure-caption">Fig. 1. Home page of the <em>Kuruna</em> interactive key.</p>
                </li>

                <li>
                    <strong>Major character group selection page</strong>
                    <p>
                        The end-user can first click on the “Interactive key” menu which directs the user to a web page that lists the nine major types of characters for temperate woody bamboos (Fig. 2).
                    </p>
                    <img src="Images/Fig2.jpg" />
                    <p class="figure-caption">Fig. 2. Major character selection page.</p>

                    <p>
                        The user must select which major character groups are present in the specimen that needs to be identified. The program will use this selection to display only the detailed characters
                        and their character states for each major group (e.g., if the specimen does not have flowers, only the vegetative characters will be displayed). This page also contains an abstract
                        drawing of a typical bamboo plant with the major characters labeled. By clicking the next button on this web page, the user will be directed to the third page.

                    </p>

                </li>

                <li><strong>Detailed interactive key of <em>Kuruna</em></strong>

                    <p>
                        This serves as the detailed interactive key for <em>Kuruna</em>. The characters and character states corresponding to the major character groups that the user selected are in the top
                        left panel
                        while images of the character states are displayed in the top right panel. The bottom left and right panels display in real-time the matching and eliminated species based on the user's
                        selection of character states.

                    </p>

                    <p>
                        Another significant feature of the interactive key is that if the user selects a character state that determines the availability of another character, the user may or may not be able
                        to select the dependent character states depending on the first character state selection.

                    </p>
                    <p>
                        For example, if the user selects “Subtending bracts at the base of axis bearing the spikelet: absent”, then they would not be able to select the other character states related to subtending
                        bracts, such as subtending bract morphology (Fig. 3). Also, the <em>Kuruna</em> species with subtending bracts would also be removed from the "Selected Species" list.

                    </p>

                    <p>
                        Also, users can see an illustration or an image of the character by clicking the “View Image” button.

                    </p>

                    <img src="Images/Fig3.jpg" />
                    <p class="figure-caption">Fig. 3. Interactive key of <em>Kuruna</em>.</p>
                </li>
                <li>
                    <strong>Species information</strong>

                    <p>
                        Under “Species Info” menu, users can download descriptions of species which have recently been published in Attigala, Kathriarachchi & Clark (2016), as well as images and illustrations,
                        if they are available.

                    </p>
                    <img src="Images/Fig4.jpg" />
                    <p class="figure-caption">Fig. 4. “Species info” page.</p>

                </li>
                <li><strong>Glossary</strong>
                    <p>
                        By clicking “Glossary”, users can see written descriptions for the scientific terminology used in the interactive key of <em>Kuruna</em>.

                    </p>
                </li>
                <li><strong>Dichotomous key</strong>
                    <p>
                        Users may also select “Dichotomous Key” to see a traditional dichotomous key for the genus.

                    </p>
                </li>

            </ol>
        </div>
    </div>



</asp:Content>
