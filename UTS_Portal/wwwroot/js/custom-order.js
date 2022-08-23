jQuery(document).ready(function () {
    $('input[type="radio"]').on('change', function () {
        var name = $(this).attr('name');

        $("input[name=" + name + "]").not(':checked').each(function () {
            var func = 'removeClass';
            $('label[for="' + $(this).attr('id') + '"]')[func]('checked');
            $('label[for="hide-' + $(this).attr('id') + '"]')[func]('checked');
        });

        var func = 'addClass';
        $('label[for="' + $(this).attr('id') + '"]')[func]('checked');
        $('label[for="hide-' + $(this).attr('id') + '"]')[func]('checked');


        // Auto check all option order
        const NameInputArr = name.split("-");
        var NameOptionCb = NameInputArr[0] + "-op-" + NameInputArr[1]; // breakfast-op-@day.Day
        $('input[name="' + NameOptionCb + '"]').each(function () {
            $(this).prop('checked', 'checked');

            var func = 'addClass';
            $('label[name="' + NameOptionCb + '"]')[func]('checked');
            $('label[name="hide-' + NameOptionCb + '"]')[func]('checked');
        });

    });

    /* Check box */
    $('input[type="checkbox"]').on('change', function () {
        var func = 'addClass';
        $('label[for="' + $(this).attr('id') + '"]')[func]('checked');
        $('label[for="hide-' + $(this).attr('id') + '"]')[func]('checked');
    });


    // Init Language
    if ($("#language").val() === "vn") {
        $(".title-en").hide();
        $(".en").hide();
        $(".en").each(function () {
            var value = $(this).attr('for');
            $(this).attr('for', "hide-" + value);
        });
    }
    else {
        $(".title-vn").hide();
        $(".vn").hide();
        $(".vn").each(function () {
            var value = $(this).attr('for');
            $(this).attr('for', "hide-" + value);
        });
    }
    


    // Language handle
    $('#language').on('change', function (e) {
        var language = $("#language").val();
        if (language === "vn") {
            $(".title-en").hide();
            $(".title-vn").show();

            $(".en").hide();
            $(".en").each(function () {
                var value = $(this).attr('for');
                if (value.indexOf("hide-") == -1) {
                    $(this).attr('for', "hide-" + value);
                }
            });

            $(".vn").show();
            $(".vn").each(function () {
                var value = $(this).attr('for');
                if (value.indexOf("hide-") != -1) {
                    $(this).attr('for', value.replace("hide-", ""));
                }
            });
        } else {
            $(".title-vn").hide();
            $(".title-en").show();

            $(".vn").hide();
            $(".vn").each(function () {
                var value = $(this).attr('for');
                if (value.indexOf("hide-") == -1) {
                    $(this).attr('for', "hide-" + value);
                }
            });

            $(".en").show();
            $(".en").each(function () {
                var value = $(this).attr('for');
                if (value.indexOf("hide-") != -1) {
                    $(this).attr('for', value.replace("hide-", ""));
                }
            });
        }
    });
});