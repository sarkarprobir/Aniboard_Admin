let isCodeView = false;

document.querySelector('[data-command="code"]').addEventListener('click', function (e) {
    e.preventDefault();

    const editor = document.getElementById('editor');

    // Optional: if using textarea
    let source = document.getElementById('sourceEditor');

    if (!source) {
        // Create textarea dynamically if not already present
        source = document.createElement('textarea');
        source.id = 'sourceEditor';
        source.style.width = '100%';
        source.style.height = '300px';
        editor.parentNode.insertBefore(source, editor.nextSibling);
    }

    if (!isCodeView) {
        // Switch to code view
        source.value = editor.innerHTML.trim();
        source.style.display = 'block';
        editor.style.display = 'none';
    } else {
        // Switch back to editor view
        editor.innerHTML = source.value.trim();
        source.style.display = 'none';
        editor.style.display = 'block';
    }

    isCodeView = !isCodeView;
});

function saveemailtemplatedata() {
    var templateid = $('#hdTemplateId').val();
    const editor = document.getElementById('editor');
    console.log(editor);
    console.log(editor.innerHTML);
    
    var data = {
        TemplateId: $('#hdTemplateId').val(),
        TemplateCode: $('#TemplateCode').val(),
        Heading: $('#Heading').val(),
        Content: editor.innerHTML,
        Remarks: $('#Remarks').val(),
        TemplateOrder: $('#TemplateOrder').val(),
        IsActive: $("input[type='radio'][name='radio']:checked").val()
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
                if (templateid != null && templateid > 0) {
                    alert('Email Template edited sucessfully');
                }
                else {
                    alert('Email Template created sucessfully');
                }

                setTimeout(function () {
                    window.location.href = '/EmailTemplate/EmailTemplateList';
                }, 1000);
            } else {
                alert(data.errmsg);
            }
        }
    });

    return false;
}
