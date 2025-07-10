$(document).ready(function () {

    setTimeout(function () {

        $('.editor').each(function () {

            if (!$(this).text().trim().length == 0) {
                $(this).addClass('active');
            } else {
                $(this).removeClass('active');
            }
            
            var setTop = $(this).siblings('.toolbar').height() + 45;
             /*alert(setTop);*/
            $(this).next('.form-label').css('top', setTop);

        });

    }, 100)




    if ($('.customEditor').length >= 1) {
        $('.customEditor').append('<div class="d-none editorSroer"></div>');
    }


    // $('.editorSroer').html($('#editor').html());
    var index = 1;
    $('.editorSroer').each(function () {

        $(this).attr('name', 'editorSroer' + index++)

        var calectedHtml = $(this).siblings('.editor').html();
        $(this).html(calectedHtml);
    })

    var colorPalette = ['000000', 'FF9966', '6699FF', '99FF66', 'CC0000', '00CC00', '0000CC', '333333', '0066FF', 'FFFFFF'];
    var forePalette = $('.fore-palette');
    var backPalette = $('.back-palette');

    for (var i = 0; i < colorPalette.length; i++) {
        forePalette.append('<a class="palette-item" data-command="forecolor" data-value="' + '#' + colorPalette[i] + '" style="background-color:' + '#' + colorPalette[i] + ';" class="palette-item"></a>');
        backPalette.append('<a class="palette-item" data-command="backcolor" data-value="' + '#' + colorPalette[i] + '" style="background-color:' + '#' + colorPalette[i] + ';" class="palette-item"></a>');
    }

    $('.toolbar a').click(function (e) {


        var command = $(this).data('command');
        if (command == 'code') {
            // alert(1);.replace(/\s\s+/g, ' ')
            // var htmlContent = $('#editorSroer').html();

            var editorNow = $(this).parents('.toolbar').siblings('.editor').html();
            $(this).parents('.toolbar').siblings('.editorSroer').html(editorNow);
            var htmlContent = $(this).parents('.toolbar').siblings('.editorSroer').html();

            var htmlEditor = `<div class="modal modalBox htmlEditor">
                                <div class="headerBox">
                                    <span class="h3">Header</span>
                                    <a class="modalCloser">&#x2715;</a>
                                </div>
                                <div class="bodyBox modal-body">
                                    <textarea rows="" class="form-control html_codeEditor" placeholder="Past HTML">` + htmlContent + `</textarea>
                                </div>
                                <div class="footerBox">
                                    <a class="btn btn-primary btnDone">Done</a>
                                </div>
                             </div>`;

            // <textarea rows="" class="form-control html_codeEditor" placeholder="Past HTML">`+ htmlContent +`</textarea>
            /*{
                *//* <span class="form-control html_codeEditor" role="textbox" contenteditable></span> *//*
            }*/

            $(this).parents('.customEditor').append(htmlEditor);
            //  $('body').append(htmlEditor);
        }
        if (command == 'h1' || command == 'h2' || command == 'p') {
            document.execCommand('formatBlock', false, command);
        }
        if (command == 'forecolor' || command == 'backcolor') {
            document.execCommand($(this).data('command'), false, $(this).data('value'));
        }
        if (command == 'createlink' || command == 'insertimage') {
            url = prompt('Enter the link here: ', 'http:\/\/');
            document.execCommand($(this).data('command'), false, url);
        } else document.execCommand($(this).data('command'), false, null);
    });



});

$(document).on('keyup', '.editor', function () {
    $(this).siblings('.editorSroer').html($(this).html());

});

$(document).on('mouseout', '.editor', function () {
    $(this).siblings('.editorSroer').html($(this).html());
});

$(document).on('click', '.modalCloser', function () {
    $(this).closest('.modalBox').addClass('modalBox_close');

    setTimeout(function () {
        // alert($('.modalBox').html())
        $(this).closest('.modalBox').remove();
    }, 500);
});

$(document).on('click', '.btnDone', function () {
    var getHtml = $(this).parents('.htmlEditor').find('textarea').val();

    $(this).parents('.htmlEditor').siblings('.editor').html(getHtml);
    // .replace(/\s\s+/g, ' ')
    $(this).parents('.htmlEditor').siblings('.editorSroer').html(getHtml);

    $(this).closest('.modalBox').addClass('modalBox_close');

    setTimeout(function () {
        $('.modalBox').remove();
    }, 500);
});

$(document).on('focusin', '.editor', function () {
    $(this).addClass('active');
}).on('blur', '.editor', function () {
    if ($(this).text().trim().length == 0) {
        $(this).removeClass('active');
    }
});