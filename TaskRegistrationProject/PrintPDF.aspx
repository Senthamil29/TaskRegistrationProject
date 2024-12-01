<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintPDF.aspx.cs" Inherits="TaskRegistrationProject.PrintPDF" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PDF</title>
    <script src="Scripts/bootstrap.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
    <script src="toastr.min.js" type="text/javascript"></script>
    <link href="toastr.min.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        function downloadPDF() {
            __doPostBack('<%= btnPrint.UniqueID %>', '');

            // Delay redirection after 4 seconds
            setTimeout(function () {
                window.location.href = 'UserInfo.aspx';  
            }, 4000);
        }
    </script>

    <style>
        .custom-radio-list {
            display: flex;
            gap: 15px;
        }

            .custom-radio-list input {
                margin-right: 10px;
            }

            .custom-radio-list label {
                margin-right: 100px;
            }

        .highlightedtext {
            color: blue;
        }
    </style>
</head>
<body>
    <form id="form" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <div class="container mt-5">
            <h2 class="text-center">Registration Details</h2>
            <div class="row justify-content-end">
                <div class="col-sm-3 text-end">
                    <asp:Label ID="lblDate" CssClass="form-label" runat="server" Text="Date "></asp:Label>
                </div>
                <div class="col-sm-3 text-end">
                    <asp:TextBox ID="txtDate" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>
                </div>
            </div>

            <!-- Personal Information Section -->
            <div class="mb-3">
                <h4>Personal Information</h4>
                <div class="row align-items-center mb-3">
                    <div class="col-md-2">
                        <asp:Label ID="lblName" runat="server" Text="Name" CssClass="form-label"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                    </div>
                </div>
                <div class="row align-items-center mb-3">
                    <div class="col-md-2">
                        <asp:Label ID="lblDOB" runat="server" Text="Date of Birth" CssClass="form-label"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtDOB" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                    </div>
                </div>
            </div>

            <!-- Contact Information Section -->
            <div class="mb-3">
                <h4>Contact Information</h4>
                <div class="row align-items-center mb-3">
                    <div class="col-md-2">
                        <asp:Label ID="lblAddress" runat="server" Text="Address" CssClass="form-label"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                    </div>
                </div>

                <div class="row align-items-center mb-3">
                    <div class="col-md-2">
                        <asp:Label ID="lblCity" runat="server" Text="City" CssClass="form-label"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblState" runat="server" Text="Province/State" CssClass="form-label"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtState" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                    </div>
                </div>

                <div class="row align-items-center mb-3">
                    <div class="col-md-2">
                        <asp:Label ID="lblZip" runat="server" Text="Zip Code" CssClass="form-label"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtZip" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblCountry" runat="server" Text="Country" CssClass="form-label"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtCountry" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                    </div>
                </div>

                <div class="row align-items-center mb-3">
                    <div class="col-md-2">
                        <asp:Label ID="lblPhone" runat="server" Text="Phone" CssClass="form-label"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblEmail" runat="server" Text="Email" CssClass="form-label"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                    </div>
                </div>
            </div>


            <!-- Membership Type Section -->
            <div class="row align-items-center mb-3">
                <div class="col-md-2">
                    <asp:Label ID="lblMembership" runat="server" Text="Membership Type" CssClass="form-label"></asp:Label>
                </div>
                <div class="col-md-10">
                    <asp:TextBox ID="txtMembership" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                </div>
            </div>

            <!-- Course Information Section -->
            <div class="mb-3">
                <h4>Course Information</h4>

                <div class="row align-items-center mb-3">
                    <div class="col-md-2">
                        <asp:Label ID="lblCourseName" runat="server" Text="Course Name" CssClass="form-label"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtCourse" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>

                    </div>
                </div>

                <div class="row align-items-center mb-3">
                    <div class="col-md-2">
                        <asp:Label ID="lblPayment" runat="server" Text="Payment Details" CssClass="form-label"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtPayment" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                    </div>
                </div>

                <div class="row align-items-center mb-3">
                    <div class="col-md-2">
                        <asp:Label ID="lblComments" runat="server" Text="Comments" CssClass="form-label"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" CssClass="form-control" Rows="3" Enabled="false"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="text-center">
                <asp:Button ID="btnPrint" runat="server" Text="Download PDF" CssClass="btn btn-primary" OnClick="btnPrint_Click" OnClientClick="downloadPDF(); return false;" />
            </div>
            <br />
        </div>
    </form>
</body>
</html>
