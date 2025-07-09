document.querySelector('.toggle-password').addEventListener('click', function () {
    const input = document.querySelector('#userpassword');
    const type = input.getAttribute('type') === 'password' ? 'text' : 'password';
    input.setAttribute('type', type);
    this.classList.toggle('fa-eye');
    this.classList.toggle('fa-eye-slash');
});

function saveuserdata() {
    var uid = $('#userId').val();
    var data = {
        CustomerId: $('#userId').val(),
        FirstName: $('#firstName').val(),
        LastName: $('#lastName').val(),
        Email: $('#email').val(),
        Password: $('#userpassword').val(),
        PhoneNumber: $('#phone').val(),
        Address: $('#Address').val(),
        City: $('#City').val(),
        State: $('#State').val(),
        ZipCode: $('#ZipCode').val(),
        Country: $('#Country').val(),
        IsActive: $("input[type='radio'][name='radio']:checked").val()
    };
    var formData = new FormData();
    formData.append('data', JSON.stringify(data));
    $.ajax({
        type: "POST",
        url: '/User/SaveUser',
        data: formData,
        dataType: 'json',
        contentType: false,
        processData: false,
        success: function (data) {
            if (data.ok) {
                if (uid != null && uid != '') {
                    alert('User edited sucessfully');
                }
                else {
                    alert('User created sucessfully');
                }
                
                setTimeout(function () {
                    window.location.href = '/User/UserList';
                }, 1000);
            } else {
                alert(data.errmsg);
            }
        }
    });

    return false;
}