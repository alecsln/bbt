// Nguyen Thanh Binh - 0923.686.993 - Thanhbinh101287@gmail.com - Y!M: Thanhbinh101287

function loadDefault()
{
    selected();
    checked();
	datetime('datetime', 'VN');
	scrollTop('onTop');

	//close panel
	$('.full').click(function () {
	    $('.full').hide();
	    $('.panel').hide();
	});

    //hide menu emtry
	$('.menu').each(function (e) {
	    if ($(this).find('li').size() == 0)
	        $(this).hide();
	});
	$('.menu h3').click(function () {
	    if ($(this).hasClass('hide'))
	        $(this).removeClass('hide');
        else
	        $(this).addClass('hide');
	    $(this).parent().find('ul').slideToggle();
	});
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

function UrlSeo(tmp) {
    if (!tmp) return '';
    var str = "áàảãạâấầẩẫậăắằẳẵặđéèẻẽẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶĐÉÈẺẼẸÊẾỀỂỄỆÍÌỈĨỊÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÚÙỦŨỤƯỨỪỬỮỰÝỲỶỸỴqwertyuiopasdfghjklzxcvbnm1234567890QWERTYUIOPASDFGHJKLZXCVBNM";
    str += "aaaaaaaaaaaaaaaaadeeeeeeeeeeeiiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAADEEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYYqwertyuiopasdfghjklzxcvbnm1234567890QWERTYUIOPASDFGHJKLZXCVBNM";
    var arr = tmp.split("");
    for (var i = 0; i < arr.length; i++) {
        if (str.indexOf(arr[i]) < 0) {
            arr[i] = "-";
        }
    }

    tmp = ReplaceSpace(arr.join("").toLowerCase());
    if (tmp.indexOf("-") == 0)
        tmp = tmp.substring(1, tmp.length);
    if (tmp.lastIndexOf("-") == tmp.length - 1)
        tmp = tmp.substring(0, tmp.length - 1);
    return tmp;
}

function ReplaceSpace(str) {
    if (str.indexOf("--") > -1) {
        str = str.replace(/--/g, '-');
        return ReplaceSpace(str);
    }
    return str;
}

function selected()
{
    $("select").each(function () {
        var title = $(this).attr("title");
        if(title != null)
        {
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

function checked() {
    $('input[type="radio"], input[type="checkbox"]').each(function () {
        var title = $(this).attr("title");
        var value = $(this).val();
        if (title != null && title.indexOf(value) >= 0)
            $(this).attr("checked", true);
    });
}

function scrollTop(where)
{
	$('#'+where).click(function() {
		$('html, body').animate({scrollTop: '0px'}, 500);
		return false;
	});
}

function editorBasic(id)
{
    var editor = CKEDITOR.replace(id,
	{
	    language: 'vi',
        width:"95%",
		toolbar :
		[
			 ['Source', '-', 'Bold', 'Italic', 'Underline', 'Strike','PasteFromWord'],
             ['Link', 'Unlink'],
			 ['NumberedList','BulletedList','-','Outdent','Indent'],
			 ['JustifyLeft','JustifyCenter','JustifyRight','JustifyBlock'],
			 ['TextColor', 'BGColor'], ['FontSize'],
             ['Maximize', 'ShowBlocks']
		]
	});
}

function datetime(where, lang) {
    var date = new Date();
    var dd = date.getDate();
    var mm = date.getMonth();
    var yyyy = date.getFullYear();
    var yy = date.getYear();
    var day = date.getDay();
    
    var day_vn = new Array("Chủ nhật", "Thứ hai", "Thứ ba", "Thứ tư", "Thứ năm", "Thứ sáu", "Thứ bẩy");
    var moth_vn = mm + 1;
    var day_en = new Array("Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday");
    var month_en = new Array("January", "February", "March", "April", "May", "June", "August", "September", "October", "November", "December");

    if (lang == 'VN')
        $('#' + where).append(day_vn[day] + ', ' + dd + '/' + moth_vn + '/' + yyyy);
    else if (lang == 'EN')
        $('#' + where).append(day_en[day] + ', ' + month_en[mm] + ' ' + dd + ', ' + yyyy);
}

function SearchForm() {
    $('input:checkbox[id="ckCheckAll"]').click(function () {
        $('input:checkbox[title^='+$(this).val()+']').attr('checked', this.checked);
        $('input:checkbox[id="ckCheckAll"]').attr('checked', this.checked);
    });

    $('select[id="show"],select[id="page"]').change(function () {
        window.location.href = $(this).find("option:selected").attr("title");
    });

    $('.table-list input,.table-list select').change(function () {
        if($(this).attr('title')!='ckID')
            $(this).parent().parent().find('input[title="ckID"]').attr('checked',true);
    });
}

function CheckAllPermission() {
    $('input[id="ckPermission"]').change(function () {
        var name = $(this).attr("name");
        $('input[lang="' + name + '"]').attr("checked", this.checked);
        $('input[lang="' + name + '"]').trigger('change');
    });
}

function ActionForm(action)
{
    $('button[id="btnActionUpdate"]').click(function () {
        $('#frmList').attr('method', "POST");
        $('#frmList').attr('action',action+"/Update");
        //$('input:checkbox[title="ckID"]').attr('checked', true);
        $('#frmList').submit();
    });

    $('button[id="btnActionUpdatePermission"]').click(function () {
        $('#frmList').attr('method', "POST");
        $('#frmList').attr('action', action + "/UpdatePermission");
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
        $('#frmAdd').attr('method',"POST");
        $('#frmAdd').attr('action',action+"/Create");
        $('#frmAdd').submit();
    });
}

function Delete(id) {
    $('input:checkbox[value="' + id + '"]').attr('checked', true);
    $('#btnActionDelete').trigger('click');
}


function ShowUpload() {
    $('button[id="btnImage"]').click(function () {
        $('.full').show();
        $('.upload').show();
        $('#fUpload').attr("src", "../file");
        createCookie("cookieFileBack", $(this).attr("lang"), 1);
    });

    $('input[id="txtImage"]').dblclick(function () {
        $('.full').show();
        $('.upload').show();
        $('#fUpload').attr("src", "/webcms/file");
        createCookie("cookieFileBack", $(this).attr("lang"), 1);
        createCookie("cookieFileValue", $(this).val(), 1);
    });
}

function ImageDescription(sName,sSrc) {
    var oEditor = CKEDITOR.instances.txtContent;
    var src = "/uploads/" + sSrc;
    if (oEditor.mode == 'wysiwyg')
        oEditor.insertHtml('<a title="' + sName + '" href="' + src + '" class="slideshow-list" rel="slideshow-list"><img alt="' + sName + '" src="' + src + '" /></a>');
}
function ImageFile(sSrc) {
    $('#txtImage').val(sSrc);
}
function ImageList(sName, sImg) {
    var img = $('#txtSlideImage');
    if (img.val() == '')
        img.val(img.val() + sName+'\n');
    else
        img.val(img.val() + '\n' + sName+'\n');
    img.val(img.val() + sImg);
}

function ShowSearch() {
    $('#btnAddRelated').click(function () {
        $('.full').show();
        $('.upload').show();
        $('#fUpload').attr("src", "/webcms/poll/search");
        createCookie("cookieSearchBack", $(this).attr("lang"), 1);
        createCookie("cookieSearchValue", $(this).val(), 1);
    });

    $('input[id="txtPoll"]').dblclick(function () {
        $('.full').show();
        $('.upload').show();
        $('#fUpload').attr("src", "/webcms/poll/search");
        createCookie("cookieSearchBack", $(this).attr("lang"), 1);
        createCookie("cookieSearchValue", $(this).val(), 1);
    });
}
function SetPoll(ID) {
    $('#txtPoll').val(ID);
}
function SetRelated(ID) {
    if($('#txtRelated').val()=='')
        $('#txtRelated').val(ID);
    else
        $('#txtRelated').val($('#txtRelated').val()+','+ID);
}


function ShowEditor()
{
    $('button[id="btnPanel"]').click(function () {
        $('.full').show();
        $('.' + $(this).attr('title')).show();

        var oEditor = CKEDITOR.instances.txtEditor;
        var value = $('#' + $(this).attr('lang')).val();
        oEditor.setData(value);
    });
}

function CreateGroup(where, arr) {
    $.each(arr, function (val, text) {
        $('#' + where).append(new Option(text, val));
    });
}

function SetValue(value, arr, where) {
    $('#' + where + ' option').remove();
    $.each(arr, function (val, text) {
        if (val.indexOf(value + '#') >= 0) {
            var id = val.substr(value.length + 1, val.length - value.length);
            $('#' + where).append(new Option(text, id));
        }
    });
}

function getCateName(child, parent, cateVal, where) {
    if (cateVal != '') {
        $.each(child, function (val, text) {
            if (val.indexOf(cateVal) >= 0) {
                var per = val.substr(0, val.indexOf("#"));
                $.each(parent, function (val2, text2) {
                    if (val2.indexOf(per) >= 0) {
                        $('#' + where).val(text2 + " " + text);
                        return;
                    }
                });
            }
        });
    }
}

function loadTabs(id) {
    //When page loads...
    $("#" + id + " .tab_content").hide(); //Hide all content
    $("#" + id + " ul.tabs li:first").addClass("active").show(); //Activate first tab
    $("#" + id + " .tab_content:first").show(); //Show first tab content

    //On Click Event
    $("#" + id + " ul.tabs li").click(function () {

        $("#" + id + " ul.tabs li").removeClass("active"); //Remove any "active" class
        $(this).addClass("active"); //Add "active" class to selected tab
        $("#" + id + " .tab_content").hide(); //Hide all tab content

        var activeTab = $(this).find("a").attr("href"); //Find the href attribute value to identify the active tab + content
        $(activeTab).fadeIn(); //Fade in the active ID content
        return false;
    });
}