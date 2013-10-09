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



function KhongDau(strVietNamese) {
    if (!strVietNamese) return '';
    //processing Vietnamese
    var FindText = "áàảãạâấầẩẫậăắằẳẵặđéèẻẽẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶĐÉÈẺẼẸÊẾỀỂỄỆÍÌỈĨỊÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÚÙỦŨỤƯỨỪỬỮỰÝỲỶỸỴqwertyuiopasdfghjklzxcvbnm1234567890QWERTYUIOPASDFGHJKLZXCVBNM";
    var ReplText = "aaaaaaaaaaaaaaaaadeeeeeeeeeeeiiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAADEEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYYqwertyuiopasdfghjklzxcvbnm1234567890QWERTYUIOPASDFGHJKLZXCVBNM";
    var index = -1;
    var tmp = strVietNamese.split("");
    var length = tmp.length;
    for (var i = 0; i < length; i++) {
        if (i >= tmp.length) break;
        index = FindText.indexOf(tmp[i]);
        if (index >= 0) {
            tmp[i] = ReplText.substr(index, 1);
        }
        else {
            tmp[i] = "-";
        }
    }
    tmp = ReplaceSpace(tmp.join("").toLowerCase());
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
		toolbar :
		[
			 ['Source','-','Bold','Italic','Underline','Strike'],
			 ['NumberedList','BulletedList','-','Outdent','Indent'],
			 ['JustifyLeft','JustifyCenter','JustifyRight','JustifyBlock'],
			 ['TextColor','BGColor'],['FontSize']
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

    $('button[id="btnActionDelete"]').click(function () {
		if($('input:checkbox[title="ckID"]:checked').val()!= null)
		{
			if(confirm("Bạn thực sự muốn xóa?"))
			{
                $('#frmList').attr('method',"POST");
                $('#frmList').attr('action',action+"/Delete");
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
