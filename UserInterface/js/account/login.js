// Object to store user input login data
const userLoginData = {
    tag_name: "",
    password: ""
};

// Error messages for login validation
const ERROR_MESSAGES = {
    tag_name: "Please fill in the tag name",
    password: "Please fill in the password",
};

// Function to update user login data and perform validation
function updateUserLoginData(fieldId) {
    userLoginData[fieldId] = document.getElementById(fieldId).value;
    hideError(fieldId);
}

// Function to hide errors
function hideError(fieldId) {
    const errorElement = document.getElementById(`${fieldId}_error`);
    errorElement.textContent = "";
}

// Function for login validation and sending data to the backend
function login() {
    // Validate input
    if (validateFields()) return;

    // Sending a POST request to the server
    fetch('https://localhost:7131/api/Account/Login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(userLoginData),
    }).then(response => {
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }
        return response.json();
    }).then(data => {
        // Set cookie with token
        setCookie("jwtToken", data.user_token, new Date(Date.now() + 12 * 3600 * 1000), "/");

        // Display successful login message and redirect to index page
        alert(data.message + '. Redirecting to index page.');
        window.location.href = 'index.html';
    }).catch(error => {
        // Display error message and clear input fields
        alert(data.message + '. Please check your credentials.');
        clearInputFields();
        console.error('Error:', error);
    });
}

// Function to clear input fields
function clearInputFields() {
    const inputFields = document.querySelectorAll('input[type="text"], input[type="password"], input[type="email"]');

    inputFields.forEach(input => {
        input.value = '';
    });
}

// Function to validate a field
function validateFields() {
    let is_error = false;

    // Validation for empty fields
    for (const field in userLoginData) {
        if (userLoginData[field] === "") {
            displayError(field, ERROR_MESSAGES[field]);
            is_error = true;
        }
    }

    return is_error;
}

// Function to display errors
function displayError(fieldId, errorMessage) {
    const errorElement = document.getElementById(`${fieldId}_error`);
    errorElement.textContent = errorMessage;
}