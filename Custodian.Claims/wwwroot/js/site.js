var modal = document.getElementById('modal');

// Function to close the modal
function closeModal() {
  modal.style.display = 'none';
}

// Event listener to close the modal when clicking outside the modal content
window.addEventListener('click', function (event) {
  if (event.target === modal) {
    closeModal();
  }
});


// Function to show the processing modal
function showProcessingModal() {
  const processingModal = document.getElementById('processingModal');
  processingModal.style.display = 'flex';
}

// Function to hide the processing modal
function hideProcessingModal() {
  const processingModal = document.getElementById('processingModal');
  processingModal.style.display = 'none';
}


document.getElementById('checkStatusBtn').addEventListener('click', function (event) {
  validateInput(event, 'checkStatusForm');
});

// Add an event listener for the submitClaimBtn
document.getElementById('submitClaimBtn').addEventListener('click', function (event) {
  validateInput(event, 'submitClaimForm');
});

function showTab(tabName) {
  const forms = document.querySelectorAll('form');
  const tabs = document.querySelectorAll('.tab');

  forms.forEach(form => form.classList.remove('active'));
  tabs.forEach(tab => tab.classList.remove('active'));

  const activeForm = document.getElementById(`${tabName}Form`);
  const activeTab = document.getElementById(`${tabName}`);

  activeForm.classList.add('active');
  activeTab.classList.add('active');
}

function validateInput(e, formId) {
  e.preventDefault();
  const form = document.getElementById(formId);
  if (formId === 'submitClaimForm') {
    const formElements = form.querySelectorAll('input, textarea');
    let isValid = true;

    for (const element of formElements) {
      const name = element.name;
      const value = element.value;
      const errorMessage = element.getAttribute('data-error') || `${pascalToWords(name)} is required.`;

      if (element.hasAttribute('required') && !value.trim()) {
        showAlert(errorMessage);
        isValid = false;
        break;
      } else if (name === 'emailAddress' && !validateEmail(value)) {
        showAlert('Invalid Email Address');
        isValid = false;
        break;
      } else if (name === 'phoneNumber' && !validPhoneNumber(value)) {
        showAlert('Invalid Phone Number');
        isValid = false;
        break;
      }
    }
    if (isValid) {
      showProcessingModal();
      const policyNumber = document.getElementById('policyNumber').value;
      const emailAddress = document.getElementById('emailAddress').value;
      const phoneNumber = document.getElementById('phoneNumber').value;
      const claimDescription = document.getElementById('claimDescription').value;

      const requestData = {
        PolicyNumber: policyNumber,
        EmailAddress: emailAddress,
        PhoneNumber: phoneNumber,
        Description: claimDescription
      };

      console.log(JSON.stringify(requestData));

      fetch('/api/ClaimRequest', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val(),
        },
        body: JSON.stringify(requestData),
      })
        .then(apiResponse => apiResponse.json())
        .then(response => {
          hideProcessingModal();
          if (response.isSuccessful) {
            Swal.fire({
              icon: 'success',
              title: 'Claim Request Successful!',
              text: 'Your Claim request has been received, kindly check your mailbox for more details',
            });
          }
          else {
            Swal.fire({
              icon: 'error',
              title: 'Invalid Input',
              text: response.message,
            });
          }
        })
        .catch((error) => {
          hideProcessingModal();
          console.error('Error:', error);
          // Handle errors here
        });
    }
  }

  else if (formId === 'checkStatusForm') {
    const formElements = form.querySelectorAll('input, textarea');
    let isValid = true;

    for (const element of formElements) {
      const name = element.name;
      const value = element.value;
      const errorMessage = element.getAttribute('data-error') || `${pascalToWords(name)} is required.`;

      if (element.hasAttribute('required') && !value.trim()) {
        showAlert(errorMessage);
        isValid = false;
        break;
      }
    }
    if (isValid) {
      showProcessingModal();
      const claimNumber = document.getElementById('claimNumber').value;

      fetch(`/api/CheckClaimStatus?claimNumber=${claimNumber}`, {
        method: 'Get',
        headers: {
          'Content-Type': 'application/json',
          'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val(),
        }
      })
        .then(apiResponse => apiResponse.json())
        .then(response => {
          hideProcessingModal();
          if (response.isSuccessful) {
            const modal = document.getElementById('modal');
            const policyNumberElement = document.getElementById('policyNumber');
            const statusElement = document.getElementById('status');
            const createdAtElement = document.getElementById('ceatedAt');

            const { claimNumber, policyNumber, status, createdAt } = response.data;

            policyNumberElement.textContent = response.data.policyNumber;
            statusElement.textContent = response.data.status;
            createdAtElement.textContent = createdAt;

            // Apply status styling based on the status value
            statusElement.className = getStatusClassName(status);

            // Show the modal
            modal.style.display = 'flex';

          }
          else {
            let title = '';
            if (response.statusCode === 400) {
              title = 'Invalid Input';
            }
            Swal.fire({
              icon: 'error',
              title: title,
              text: response.message,
            });
          }
        })
        .catch((error) => {
          hideProcessingModal();
          console.error('Error:', error);
          // Handle errors here
        });
    }
  }
}

// Function to get the appropriate class name for status styling
function getStatusClassName(status) {
  switch (status) {
    case 'Approved':
      return 'status-approved';
    case 'Pending':
      return 'status-pending';
    case 'Denied':
      return 'status-denied';
    default:
      return '';
  }
}

function showAlert(errorMessage) {
  Swal.fire({
	icon: "warning",
	text: errorMessage,
  });
}

function validateEmail(email) {
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  return emailRegex.test(email);
}

function validPhoneNumber(phoneNumber) {
  const numericPhoneNumber = phoneNumber.replace(/\D/g, '');
  return numericPhoneNumber.length === 11;
}


function pascalToWords(pascalString) {
  const result = pascalString.replace(/([a-z])([A-Z])/g, '$1 $2');
  return result.charAt(0).toUpperCase() + result.slice(1).toLowerCase();
}
