(function () {
    $.extend({
        manNav: function (nav) {
            var $manNav = $(nav);
            $manNav.children(".second").children("a").on("click", function () {
                $(this).next("ul").slideToggle();
                return false;
            })
        }
    });
}(jQuery));

function addKeyWord() {
    var selectedKeyword = $(".keyword").find("option:selected");
    var keyId = selectedKeyword.attr('id');
    var keyName = selectedKeyword.val();
    pwd = $("<li><input type='checkbox' checked='checked' name='keywords' value='" + keyId + "'/>" + keyName + " </li>");     // 创建的input对象
    $(".addKeyword").append(pwd);
}

