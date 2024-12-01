<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserInfo.aspx.cs" Inherits="TaskRegistrationProject.UserInfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Information</title>
    <script src="Scripts/bootstrap.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
    <script src="toastr.min.js" type="text/javascript"></script>
    <link href="toastr.min.css" rel="stylesheet" type="text/css" />
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

        #lblRegisterID {
            font-weight: 700;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="false"></asp:ScriptManager>
        <div class="container mt-5">
            <div runat="server" id="searchRegister" visible="true">
                <div class="row align-items-center mb-3">
                    <div class="col-md-2">
                        <asp:Label runat="server" for="txtSearch" class="form-label">Registration ID </asp:Label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
                    </div>
                </div>
            </div>

            <div runat="server" id="registerIdMsg" visible="false">
                <div class="row align-items-center mb-3">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
                        <asp:Label runat="server" ID="lblMsg" Text="" class="form-label fw-bold"></asp:Label>
                    </div>
                    <div class="col-md-3" runat="server" id="registerBtn" visible="true">
                        <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-outline-info me-2" OnClick="btnEdit_Click" />
                        OR
                        <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btn btn-outline-secondary" OnClick="btnPrint_Click" />
                    </div>
                </div>
            </div>

            <asp:UpdatePanel ID="UpdatePanel" runat="server">
                <ContentTemplate>
                    <div id="registerDetails" runat="server" visible="false">
                        <div class="row align-items-center mb-3">
                            <div class="col-md-12 text-center">
                                <h3>Registration Details</h3>
                            </div>
                        </div>
                        <div class="row mb-3">
                            <div class="col-md-12 text-end">
                                <asp:Label ID="lblRegisterID" runat="server" CssClass="form-label"></asp:Label>
                            </div>
                        </div>
                        <!-- Personal Information Section -->
                        <div class="mb-3">
                            <h4>Personal Information</h4>
                            <div class="row align-items-center mb-3">
                                <div class="col-md-2">
                                    <asp:Label ID="lblName" runat="server" Text="Name" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="col-md-7">
                                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control" Placeholder="Enter your name"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" ErrorMessage="Name is required." ForeColor="Red" CssClass="text-danger" Display="Dynamic" />
                                    <asp:RegularExpressionValidator ID="revName" runat="server" ControlToValidate="txtName" ValidationExpression="^[A-Za-z\s]+$" ErrorMessage="Only letters and spaces are allowed." ForeColor="Red" CssClass="text-danger" Display="Dynamic" />
                                </div>
                            </div>
                            <div class="row align-items-center mb-3">
                                <div class="col-md-2">
                                    <asp:Label ID="lblDOB" runat="server" Text="Date of Birth" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="col-md-7">
                                    <asp:TextBox ID="txtDOB" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <asp:RequiredFieldValidator ID="rfvDate" runat="server" ControlToValidate="txtDOB" ErrorMessage="DOB is required." ForeColor="Red" CssClass="text-danger" Display="Dynamic" />

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
                                <div class="col-md-7">
                                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" Placeholder="Enter your address"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ControlToValidate="txtAddress" ErrorMessage="Address is required." ForeColor="Red" CssClass="text-danger" Display="Dynamic" />

                                </div>
                            </div>

                            <div class="row align-items-center mb-3">
                                <div class="col-md-2">
                                    <asp:Label ID="lblCity" runat="server" Text="City" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlCity" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlCity_SelectedIndexChanged">
                                        <asp:ListItem Text="--Select City--" Value="" />
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblState" runat="server" Text="Province/State" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtState" runat="server" CssClass="form-control" Placeholder="State" Enabled="false"></asp:TextBox>
                                </div>
                            </div>

                            <div class="row align-items-center mb-3">
                                <div class="col-md-2">
                                    <asp:Label ID="lblZip" runat="server" Text="Zip Code" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtZip" runat="server" CssClass="form-control" Placeholder="Enter zip code" MaxLength="6"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvZip" runat="server" ControlToValidate="txtZip" ForeColor="Red" ErrorMessage="Zip Code is required." CssClass="text-danger" Display="Dynamic" />

                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblCountry" runat="server" Text="Country" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtCountry" runat="server" CssClass="form-control" Placeholder="Country" Enabled="false"></asp:TextBox>
                                </div>
                            </div>

                            <div class="row align-items-center mb-3">
                                <div class="col-md-2">
                                    <asp:Label ID="lblPhone" runat="server" Text="Phone" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="col-md-7">
                                    <asp:TextBox ID="txtPhone" TextMode="Phone" runat="server" CssClass="form-control" Placeholder="Enter phone number" AutoPostBack="true" MaxLength="10"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <asp:RequiredFieldValidator ID="rfvPhone" runat="server" ControlToValidate="txtPhone" ErrorMessage="Phone number is required." ForeColor="Red" CssClass="text-danger" Display="Dynamic" />
                                    <asp:RegularExpressionValidator ID="revPhone" runat="server" ControlToValidate="txtPhone" ValidationExpression="^\d{10}$" ErrorMessage="Please enter a valid 10-digit phone number." ForeColor="Red" CssClass="text-danger" Display="Dynamic" />

                                </div>
                            </div>
                            <div class="row align-items-center mb-3">
                                <div class="col-md-2">
                                    <asp:Label ID="lblEmail" runat="server" Text="Email" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="col-md-7">
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Placeholder="Enter email"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ForeColor="Red" ErrorMessage="Email is required." CssClass="text-danger" Display="Dynamic" />
                                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$" ForeColor="Red" ErrorMessage="Please enter a valid email address." CssClass="text-danger" Display="Dynamic" />

                                </div>
                            </div>
                        </div>


                        <!-- Membership Type Section -->
                        <div class="row align-items-center mb-3">
                            <div class="col-md-2">
                                <asp:Label ID="lblMembership" runat="server" Text="Membership Type" CssClass="form-label"></asp:Label>
                            </div>
                            <div class="col-md-7">
                                <asp:RadioButtonList ID="rblMembership" runat="server" RepeatDirection="Horizontal" CssClass="custom-radio-list">
                                    <asp:ListItem Text="Regular" Value="Regular"></asp:ListItem>
                                    <asp:ListItem Text="Premium" Value="Premium"></asp:ListItem>
                                    <asp:ListItem Text="VIP" Value="VIP"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <div class="col-md-3">
                                <asp:RequiredFieldValidator ID="rfvMembership" runat="server" ControlToValidate="rblMembership" ForeColor="Red" ErrorMessage="Membership Type is required." CssClass="text-danger" Display="Dynamic" />

                            </div>
                        </div>

                        <!-- Course Information Section -->
                        <div class="mb-3">
                            <h4>Course Information</h4>

                            <div class="row align-items-center mb-3">
                                <div class="col-md-2">
                                    <asp:Label ID="lblCourseName" runat="server" Text="Course Name" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="col-md-7">
                                    <asp:DropDownList ID="ddlCourse" runat="server" CssClass="form-select" AutoPostBack="true">
                                        <asp:ListItem Text="--Select Course--" Value="0" />
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="row align-items-center mb-3">
                                <div class="col-md-2">
                                    <asp:Label ID="lblPayment" runat="server" Text="Payment Details" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="col-md-7">
                                    <asp:RadioButtonList ID="rblPayment" runat="server" RepeatDirection="Horizontal" CssClass="custom-radio-list">
                                        <asp:ListItem Text="Cash" Value="Cash"></asp:ListItem>
                                        <asp:ListItem Text="Cheque" Value="Cheque"></asp:ListItem>
                                        <asp:ListItem Text="Credit Card" Value="CreditCard"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                <div class="col-md-3">
                                    <asp:RequiredFieldValidator ID="rfvPayment" runat="server" ControlToValidate="rblPayment" ForeColor="Red" ErrorMessage="Payment Detail is required." CssClass="text-danger" Display="Dynamic" />

                                </div>
                            </div>

                            <div class="row align-items-center mb-3">
                                <div class="col-md-2">
                                    <asp:Label ID="lblComments" runat="server" Text="Comments" CssClass="form-label"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" CssClass="form-control" Rows="3"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="text-center">
                            <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-primary me-3" OnClick="btnUpdate_Click" />
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-danger" OnClick="btnDelete_Click" CausesValidation="false" />

                        </div>
                        <br />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
