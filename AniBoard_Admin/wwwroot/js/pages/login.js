const formSlider = document.getElementById('form-slider');
const showForgot = document.getElementById('show-forgot');
const backToLogin = document.getElementById('back-to-login');
//const onsubmit = document.getElementById('login-submit');
showForgot.addEventListener('click', function (e) {
    e.preventDefault();
    formSlider.style.transform = 'translateX(-100%)';
});

backToLogin.addEventListener('click', function (e) {
    e.preventDefault();
    formSlider.style.transform = 'translateX(0%)';
});

document.querySelector('.toggle-password').addEventListener('click', function () {
    const input = document.querySelector('#userpassword');
    const type = input.getAttribute('type') === 'password' ? 'text' : 'password';
    input.setAttribute('type', type);
    this.classList.toggle('fa-eye');
    this.classList.toggle('fa-eye-slash');
});

function sendReset() {
    const email = document.getElementById('reset-email').value;
    alert('Reset link sent to: ' + email);
}

//onsubmit.addEventListener('click', function (e) {
//    e.preventDefault();
//    alert('button clicked');
//    //formSlider.style.transform = 'translateX(0%)';
//});

function fromValid() {
    var email = $('#username').val();
    var password = $('#userpassword').val();
    var valid = false;
    if (email == '') {
        $('#invalidEmail').html("Please enter email");
        $('#invalidEmail').css("display", "block");
    }
    if (password == '') {
        $('#invalidPassword').html("Please enter password");
        $('#invalidPassword').css("display", "block");
    }

    if (email != '' && password != '') {
        return true;
    } else {
        return false;
    }
}