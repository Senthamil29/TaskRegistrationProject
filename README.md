**Project Overview**
    This project implements a Registration Form with validation on all the fields. It follows a three-tier architecture for better separation of concerns and scalability. The architecture consists of:

*1. Presentation Layer:* The user interface where data is captured via a registration form and dropdowns are populated with data from the database.
*2. Business Logic Layer:* The layer that handles the validation, processing, and PDF generation based on the user input.
*3. Data Access Layer:* Manages interaction with the database, including fetching dropdown values and storing the registration data.
     The project validates all form fields and ensures correct input before submission. It also provides appropriate error messages for invalid inputs.

**Features**
  *Three-Tier Architecture:* Separation of concerns for better maintainability.
  *Field Validation:* All fields in the registration form are validated before submission.
  *Dropdown Binding:* Dropdown fields are populated from the database with relevant data (e.g., list of cities, course, etc.).
  *Search by Registration ID:* Users can search for existing records based on the registration ID.
  *PDF Generation:* Once the Registration ID is found, a PDF is generated for the corresponding registration details.
  *User-friendly UI:* The registration form is simple and easy to use.

**Technologies Used**
  *Frontend:* ASP.NET Web Forms (with HTML, CSS, Bootstrap,Javascript)
  *Backend:* C# (ASP.NET)
  *Database:* Oracle
  *Validation:* Custom and built-in validation for form fields.


