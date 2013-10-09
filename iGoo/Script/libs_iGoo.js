// Nguyen Thanh Binh - 0923.686.993 - Thanhbinh101287@gmail.com - Y!M: Thanhbinh101287

function scrollTop(where) {
    $('#' + where).click(function () {
        $('html, body').animate({ scrollTop: '0px' }, 500);
        return false;
    });
}

function checked() {
    $('input[type="radio"], input[type="checkbox"]').each(function () {
        var title = $(this).attr("title");
        var value = $(this).val();
        if (title != null && title.indexOf(value) >= 0)
            $(this).attr("checked", true);
    });
}

function poll() {
    $('button:[id="btnPollSubmit"]').click(function () {
        var id = $(this).attr("lang");
        var check = $('input[name="PollID-' + id + '"]:checked').val();
        if (check != null) {
            var url = '/poll?id=' + id + '&vote=' + check;
            $.fancybox({
                'href': url,
                'transitionIn': 'elastic',
                'transitionOut': 'elastic',
                'zoomOpacity': true,
                'overlayShow': true,
                'zoomSpeedIn': 500,
                'zoomSpeedOut': 500
            });
        }
        else
            alert("Bạn hãy chọn câu trả lời!");
    });

    $('button:[id="btnPollView"]').click(function () {
        var id = $(this).attr("lang");
        var url = '/poll?id=' + id;
        $.fancybox({
            'href': url,
            'transitionIn': 'elastic',
            'transitionOut': 'elastic',
            'zoomOpacity': true,
            'overlayShow': true,
            'zoomSpeedIn': 500,
            'zoomSpeedOut': 500
        });
    });
}

function ActionForm(action) {
    $('button[id="btnActionUpdate"]').click(function () {
        $('#frmList').attr('method', "POST");
        $('#frmList').attr('action', action + "/Update");
        //$('input:checkbox[title="ckID"]').attr('checked', true);
        $('#frmList').submit();
    });

    $('button[id="btnActionDelete"]').click(function () {
        if ($('input:checkbox[title="ckID"]:checked').val() != null) {
            if (confirm("Bạn thực sự muốn xóa?")) {
                $('#frmList').attr('method', "POST");
                $('#frmList').attr('action', action + "/Delete");
                $('#frmList').submit();
            }
        }
    });

    $('button[id="btnUpdate"]').click(function () {
        $('#frmAdd').attr('method', "POST");
        $('#frmAdd').attr('action', action + "/Create");
        $('#frmAdd').submit();
    });
}

function BrtoNl(str) {
    return str.replace(/<br ?\/?>/g, "\n")
}
function NltoBr(str) {
    pat = new RegExp(String.fromCharCode(10), "g")
    return str = str.replace(pat, "<br />");
}

function HTMLToBBCode(str) {
    str = str.replace(new RegExp("<iframe width=\"500\" height=\"405\" src=\"", "g"), "[video]").replace(new RegExp("\" frameborder=\"0\" allowfullscreen=\"\"></iframe>", "g"), '[/video]');
    str = str.replace(new RegExp( "<br>", "g" ), "\n");
    str = str.replace(new RegExp( "<strong>", "g" ), "[b]").replace(new RegExp("</strong>", "g" ),"[/b]");
    str = str.replace(new RegExp( "<em>", "g" ),"[i]").replace(new RegExp( "</em>", "g" ),"[/i]");
    str = str.replace(new RegExp("<u>", "g"), "[u]").replace(new RegExp("</u>", "g"), "[/u]");
    str = str.replace(new RegExp("<per>", "g"), "[code]").replace(new RegExp("</per>", "g"), "[/code]");
    str = str.replace(new RegExp("<img src=\"", "g"), "[img]").replace(new RegExp("\">", "g"), "[/img]");
    return str;
}

function BBCodeToHTML(str) {
    str = str.replace(new RegExp("\[video\]http://youtu.be", "g"), "[video]http://www.youtube.com/embed");
    str = str.replace(new RegExp("\[video\]http://www.youtube.com/watch?v=", "g"), "[video]http://www.youtube.com/embed/");
    str = str.replace(/\[video\]/g, "<iframe width=\"500\" height=\"405\" src=\"");
    str = str.replace(/\[\/video\]/g, "\" frameborder=\"0\" allowfullscreen></iframe>");
    str = str.replace(/\[b\]/g, "<strong>").replace(/\[\/b\]/g, "</strong>");
    str = str.replace(/\[i\]/g, "<em>").replace(/\[\/i\]/g, "</em>");
    str = str.replace(/\[u\]/g, "<u>").replace(/\[\/u\]/g, "</u>");
    str = str.replace(/\[code\]/g, "<per>").replace(/\[\/code\]/g, "</per>");

    return str;
}

function selected() {
    $("select").each(function () {
        var title = $(this).attr("title");
        if (title != null) {
            title = title.split(',');
            $(this).find("option").each(function () {
                for (i = 0; i < title.length; i++)
                    if ($(this).val() == title[i])
                        $(this).attr("selected", "selected");
            });
        }
    });
    //$('html, body').animate({scrollTop: $(document).height()}, 1000).animate({scrollTop: '0px'}, 500);
}

function editorBasic(id) {
    var editor = CKEDITOR.replace(id,
	{
	    toolbar:
		[
			 ['Source', '-', 'Bold', 'Italic', 'Underline', 'Strike'],
			 ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent'],
			 ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
			 ['TextColor', 'BGColor'], ['FontSize']
		]
	});
}

function stickly(id, top) {
    var id = $('#' + id + '');
    var offset = id.offset();
    $(window).scroll(function () {
        if ($(window).scrollTop() > offset.top) {
            id.stop().animate({
                marginTop: $(window).scrollTop() - offset.top + top
            });
        } else {
            id.stop().animate({
                marginTop: 0
            });
        };
    });
}


//kiem tra browser cho dung cookie khong
function checkBrowserEnableCookie() {
    var cookieEnabled = (navigator.cookieEnabled) ? true : false

    //if not IE4+ nor NS6+
    if (typeof navigator.cookieEnabled == "undefined" && !cookieEnabled) {
        document.cookie = "testcookie"
        cookieEnabled = (document.cookie.indexOf("testcookie") != -1) ? true : false
    }

    if (cookieEnabled) return true;
    else return false;
}

//tao cookie
function createCookie(name, value, days) {
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        var expires = "; expires=" + date.toGMTString();
    }
    else var expires = "";
    document.cookie = name + "=" + value + expires + "; path=/";
}

//doc cookie
function readCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return "";
}

//xoa bo cookie
function eraseCookie(name) {
    createCookie(name, "", -1);
}

//doc so luong san pham dang co trong gio hang
function countShoppingCart(name) {
    //tao cookie gio hang neu cua co
    if (readCookie(name) == "") {
        document.getElementById('count_shopping_cart').innerHTML = "<b>(0)</b>";
    } else {
        //add gia tri vao shopping cart
        var current_cart = readCookie(name);
        var ca = current_cart.split('$$');
        number_product = ca.length - 1;
        document.getElementById('count_shopping_cart').innerHTML = "<b>(" + number_product + ")</b>";
    }
}

//them san pham vao gio hang
function addToShoppingCart(productId, quantity, type) {
    //tao cookie gio hang neu cua co
    if (readCookie('shopping_cart') == null) {
        createCookie('shopping_cart', '', 1);
    }
    //add gia tri vao shopping cart
    var current_cart = readCookie('shopping_cart');

    //kiem tra xem da duoc them vao cart hay chua
    if (current_cart.search('\\$\\$' + productId + '#' + type + '#') == -1) {
        var new_cart = current_cart + '$$' + productId + '#' + type + '#' + quantity;
        createCookie('shopping_cart', new_cart, 1);
        //doc lai gia tri moi
        countShoppingCart('shopping_cart');
        alert('Đã thêm vào giỏ hàng!');
    } else {
        alert('Sản phẩm này đã có trong giỏ hàng \nVui lòng xem chi tiết giỏ hàng để chỉnh sửa hoặc xóa bớt sản phẩm!');
    }
}

//Thêm sản phẩm giỏ hàng và chuyển tới trang giỏ hàng
function addToCartRedirect(productId, quantity, type) {
    if (readCookie('shopping_cart') == null) {
        createCookie('shopping_cart', '', 1);
    }
    var current_cart = readCookie('shopping_cart');

    if (current_cart.search('\\$\\$' + productId + '#' + type + '#') == -1) {
        var new_cart = current_cart + '$$' + productId + '#' + type + '#' + quantity;
        createCookie('shopping_cart', new_cart, 1);
        countShoppingCart('shopping_cart');
        window.location = "/order";
    } else {
        alert('Sản phẩm này đã có trong giỏ hàng \nVui lòng xem chi tiết giỏ hàng để chỉnh sửa hoặc xóa bớt sản phẩm!');
        window.location = "/order";
    }
}

function checkItemInCart(productId, type) {
    var current_cart = readCookie('shopping_cart');
    //kiem tra xem da duoc them vao cart hay chua
    if (current_cart.search('\\$\\$' + productId + '#' + type + '#') != -1) {
        return 'Đang trong giỏ hàng!';
    }
}

//xoa bo item trong cart
function deleteFromCart(productId, quantity, type) {
    if (confirm('Bỏ mua sản phẩm này khỏi giỏ hàng? ')) {
        var current_cart = readCookie('shopping_cart');
        new_cart = current_cart.replace('\$\$' + productId + '#' + type + '#' + quantity, "");
        createCookie('shopping_cart', new_cart, 1);
        countShoppingCart('shopping_cart');
        //ok nhay ve trang gio hang moi
        window.location = "/order";
    }
}

//cap nhat so luong san pham trong cart
function updateCart(productId, quantity, type) {
    var newquantity = document.getElementById("item_"+productId).value;
    newquantity = parseInt(newquantity);
    if (newquantity < 1) {
        //Nếu cập nhật số lượng < 1 thì coi như là xóa sản phẩm khỏi cart
        deleteFromCart(productId, quantity,type);
    } else {
        if (newquantity > 99) {
            alert('Bạn chỉ được phép mua tối đa số lượng 99 cho mỗi sản phẩm!');
            newquantity = 99;
        }
        var current_cart = readCookie('shopping_cart');
        new_cart = current_cart.replace('\$\$' + productId + '#' + type + '#' + quantity, '$$$' + productId + '#' + type + '#' + newquantity);
        createCookie('shopping_cart', new_cart, 1);
        countShoppingCart('shopping_cart');
        //ok nhay ve trang gio hang moi
        window.location = "/order";
    }
}

//Luu danh sach san pham da xem
function addToViewHistory(productId) {
    if (readCookie('product_history') == null) {
        createCookie('product_history', ',', 1);
    }
    var current_list = readCookie('product_history');
    if (current_list.search(',' + productId + ',') == -1) {
        var new_list = "," + productId + current_list;
        createCookie('product_history', new_list, 1);
    }
    alert("Lưu sản phẩm thành công!");
}