function searchemailtemplate() {
    var searchText = $('#emailtemplate_search').val();
    if (searchText != null && searchText != '') {
        //alert(searchText);
        window.location.href = '/EmailTemplate/EmailTemplateList?q=' + searchText;
    }
    else {
        window.location.href = '/EmailTemplate/EmailTemplateList';
    }
}

function showaddedittemplate_modal(templateId) {
    $("#emailtemplate_body").empty();

    $.ajax({
        url: "/EmailTemplate/ShowAddEditEmailTemplateModal",
        type: "GET",
        data: { "templateId": templateId },
        datatype: "json",
        success: function (data) {
            $("#emailtemplate_body").html(data);
        },
    });
}


function deleteemailtemplate(templateId) {
    if (confirm("Are you sure you want to delete this email template?")) {
        var data = {
            TemplateId: templateId,
            isDelete: 1
        };
        var formData = new FormData();
        formData.append('data', JSON.stringify(data));
        $.ajax({
            type: "POST",
            url: '/EmailTemplate/SaveEmailTemplate',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (data) {
                if (data.ok) {
                    alert('Email template deleted sucessfully');
                    setTimeout(function () {
                        window.location.href = '/EmailTemplate/EmailTemplateList';
                    }, 1000);
                } else {
                    alert(data.errmsg);
                }
            }
        });
    }


    return false;


}