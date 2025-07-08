document.querySelector('.toggle-password').addEventListener('click', function () {
    const input = document.querySelector('#userpassword');
    const type = input.getAttribute('type') === 'password' ? 'text' : 'password';
    input.setAttribute('type', type);
    this.classList.toggle('fa-eye');
    this.classList.toggle('fa-eye-slash');
});

function saveadminuserdata() {

    var data = {
        adminId: $('#userId').val(),
        firstName: $('#firstName').val(),
        lastName: $('#lastName').val(),
        email: $('#email').val(),
        userPassword: $('#userpassword').val(),
        isActive: $("input[type='radio'][name='radio']:checked").val()
    };
    var formData = new FormData();
    formData.append('data', JSON.stringify(data));
    $.ajax({
        type: "POST",
        url: '/AdminUser/SaveAdminUser',
        data: formData,
        dataType: 'json',
        contentType: false,
        processData: false,
        success: function (data) {
            if (data.ok) {

                alert('User created sucessfully');
                setTimeout(function () {
                    window.location.href ='/AdminUser/AdminUserList';
                }, 1000);
            } else {
                alert(data.errmsg);
            }
        }
    });

    return false;
}