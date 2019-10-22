function initializeRemotelyValidatingElementsWithAdditionalFields($form) {
    var remotelyValidatingElements = $form.find("[data-val-remote]");

    $.each(remotelyValidatingElements, function (i, element) {
        var $element = $(element);

        var additionalFields = $element.attr("data-val-remote-additionalfields");

        if (additionalFields.length == 0) return;

        var rawFieldNames = additionalFields.split(",");

        var fieldNames = $.map(rawFieldNames, function (fieldName) { return fieldName.replace("*.", ""); });

        $.each(fieldNames, function (i, fieldName) {
            $form.find("#" + fieldName).change(function () {
                // force re-validation to occur
                $element.removeData("previousValue");
                $element.valid();
            });
        });
    });
}

$(document).ready(function () {
    if ($("#registrationForm").length) {
        initializeRemotelyValidatingElementsWithAdditionalFields($("#registrationForm"));
    }


    $(document).on('click', '.submitUserRegistration', function (e) {
        var $btn = $(this);
        $btn.button('loading');
        $btn.prop('disabled', true);

        var frm = $(e.target).closest('form');
        frm.validate();
        if (!frm.valid()) {
            return;
        }
        $.ajax({
            url: '/registration/Validate/',
            data: frm.serialize(),
            type: 'POST',
            success: function (data) {
                $('#formContainer').html(data);
            },
            error: function (data) {
                $('#formContainer').html(data);

            },
            complete: function (data) {
                $btn.button('reset');
                $btn.prop('disabled', false);

            }
        });
    });


    $(document).on('click', '#btnValidateReferenceCode', function (e) {

        var $btn = $(this);
        $btn.button('loading');
        $btn.prop('disabled', true);
        var objdata = JSON.stringify({
            ReferenceCode: jQuery('#ReferenceCode').val()

        });
        var frm = $(e.target).closest('form');
        console.log(objdata);
        $.ajax({
            url: '/registration/ValidateCodeNew/',
            data: frm.serialize(),
            type: 'POST',
            success: function (data) {
                $('#formContainer').html(data);
            },
            error: function (data) {
                $('#formContainer').html(data);
            },
            complete: function (data) {
                $btn.button('reset');
                $btn.prop('disabled', false);
            }
        });

    });

});

