// document monitor
$(document).ready(function () {
    layui.element;
    layui.form;
    var laydate = layui.laydate;
    var layer = layui.layer;
    MobileTreeButtonMonior();
    AjaxMonitor();
    SidebarItemedMonitor();
});

function MobileTreeButtonMonior() {
    //手机设备的简单适配
    $('.site-tree-mobile').on('click', function () {
        $('body').addClass('site-mobile');
    });

    $('.site-mobile-shade').on('click', function () {
        $('body').removeClass('site-mobile');
    });
}

// ajax monitor
function AjaxMonitor() {
    var index
    $(document).ajaxStart(function () {
        //$('.ajax_loading').css({ 'display': 'block' });
        index = layer.load(2, { shade: 0.2 });
    });

    $(document).ajaxComplete(function () {
        //$('.ajax_loading').css({ 'display': 'none' });
        layer.close(index);
    });

    $(document).ajaxError(function (event, request, settings, error) {
        alert('Error: ' + error);
    });
}

function LayerLoad() {
    index = layer.load(2, { shade: 0.2 });
}

// side bar item click monitor
function SidebarItemedMonitor() {
    var u = window.location.pathname;
    var a = $(`a[href="${u}"]`);
    //var a = $(url);
    $(a).parent().addClass('layui-this');
    $(a).parent().parent().parent().addClass('layui-nav-itemed');
}

function SidebarItemed(url) {
    var a = $(`a[href="${url}"]`);
    $(a).parent().addClass('layui-this');
    $(a).parent().parent().parent().addClass('layui-nav-itemed');
}

// ADUserIndex
function ADUserFindOne() {
    var name = $('#samAccount').val();
    $('#divTools').html('');
    $.ajax({
        url: "/aduser/aduserfindone",
        type: "post",
        data: { samAccount: name, __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() },
        dataType: "html",
        success: function (data) {
            $('#divTools').html(data);
        }
    });
}

function ADUserFindAll() {
    $('#divTools').html('');
    $.ajax({
        url: "/aduser/aduserfindall",
        type: "post",
        data: { __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() },
        dataType: "html",
        success: function (data) {
            $('#divTools').html(data);
        }
    });
}

function ADUserUnlock(sam) {
    layer.confirm('Confirm Unlock ?', {
        btn: ['Yes', 'Cancel'],
        btn1: function (index) {
            $.ajax({
                url: "/aduser/ADUserUnlock",
                type: "post",
                data: { samAccount: sam, __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() },
                dataType: "text",
                success: function (data) {
                    layer.msg(data);
                }
            });
            layer.close(index);
        },
        btn2: function (index) {
            layer.close(index)
        }
    });
}

function ADUserEnableOrDisable(sam) {
    layer.confirm('Account Enable or Disable ?', {
        btn: ['Enable', 'Disable', 'Cancel'],
        btn1: function (index) {
            $.ajax({
                url: "/aduser/ADUserEnable",
                type: "post",
                data: { samAccount: sam, __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() },
                dataType: "text",
                success: function (data) {
                    layer.msg(data);
                }
            });
            layer.close(index);
        },
        btn2: function (index) {
            $.ajax({
                url: "/aduser/ADUserDisable",
                type: "post",
                data: { samAccount: sam, __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() },
                dataType: "text",
                success: function (data) {
                    layer.msg(data);
                }
            });
            layer.close(index);
        },
        btn3: function (index) {
            layer.close(index);
        }
    });
}

function ADUserResetPassword(sam) {
    layer.prompt({
        formType: 1,    // 0:文本, 1:密碼, 2:多行文本
        value: '',
        title: 'Reset Password',
        area: ['800px', '350px'] //自定义文本域宽高
    }, function (value, index, elem) {

        $.ajax({
            url: "/aduser/ADUserResetPassword",
            type: "post",
            data: { samAccount: sam, newPassword: value, __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() },
            dataType: "text",
            success: function (data) {
                layer.alert(data);
            }
        });

        layer.close(index);
    });
}

function ADUserSetScriptPath(sam) {
    layer.prompt({
        formType: 0,
        value: '',
        title: 'Set LoginScript Path',
        area: ['800px', '350px']
    }, function (value, index, elem) {

        $.ajax({
            url: "/aduser/ADUserSetScriptPath",
            type: "post",
            data: { samAccount: sam, scriptPath: value, __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() },
            dataType: "text",
            success: function (data) {
                layer.alert(data);
            }
        });

        layer.close(index);
    });
}

function ADUserDelete(sam) {
    layer.confirm(sam+' Confirm Delete ?', {
        btn: ['Yes', 'Cancel'],
        btn1: function (index) {
            $.ajax({
                url: "/aduser/ADUserDelete",
                type: "post",
                data: { samAccount: sam, __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() },
                dataType: "text",
                success: function (data) {
                    layer.msg(data);
                }
            });
            layer.close(index);
        },
        btn2: function (index) {
            layer.close(index)
        }
    });
}

// ADGroupIndex
function ADGroupFindOne() {
    var name = $('#groupName').val();
    $('#divTools').html('');
    $.ajax({
        url: "/adgroup/adgroupfindone",
        type: "post",
        data: { groupName: name, __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() },
        dataType: "html",
        success: function (data) {
            $('#divTools').html(data);
        }
    });
}

function ADGroupFindAll() {
    $('#divTools').html('');
    $.ajax({
        url: "/adgroup/adgroupfindall",
        type: "post",
        data: { __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() },
        dataType: "html",
        success: function (data) {
            $('#divTools').html(data);
        }
    });
}

// ADComputerIndex
function ADComputerFindAll() {
    $('#divTools').html('');
    $.ajax({
        url: "/adcomputer/ADComputerFindAll",
        type: "post",
        data: { __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() },
        dataType: "html",
        success: function (data) {
            $('#divTools').html(data);
        }
    });
}

function ADComputerFindOne() {
    var name = $('#computerName').val();
    $('#divTools').html('');
    $.ajax({
        url: "/adcomputer/ADComputerFindOne",
        type: "post",
        data: { name: name, __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() },
        dataType: "html",
        success: function (data) {
            $('#divTools').html(data);
        }
    });
}

function ADComputerDelete(name) {
    layer.confirm('Confirm Delete ?', {
        btn: ['Yes', 'Cancel'],
        btn1: function (index) {
            $.ajax({
                url: "/adcomputer/ADComputerDelete",
                type: "post",
                data: { name: name, __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() },
                dataType: "text",
                success: function (data) {
                    layer.msg(data);
                }
            });
            layer.close(index);
        },
        btn2: function (index) {
            layer.close(index)
        }
    });
}

function ADComputerEnableOrDisable(name) {
    layer.confirm('Computer Enable or Disable ?', {
        btn: ['Enable', 'Disable', 'Cancel'],
        btn1: function (index) {
            $.ajax({
                url: "/adcomputer/ADComputerEnable",
                type: "post",
                data: { name: name, __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() },
                dataType: "text",
                success: function (data) {
                    layer.msg(data);
                }
            });
            layer.close(index);
        },
        btn2: function (index) {
            $.ajax({
                url: "/adcomputer/ADComputerDisable",
                type: "post",
                data: { name: name, __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() },
                dataType: "text",
                success: function (data) {
                    layer.msg(data);
                }
            });
            layer.close(index);
        },
        btn3: function (index) {
            layer.close(index);
        }
    });
}

function ADComputerUnlock(name) {
    layer.confirm('Confirm Unlock ?', {
        btn: ['Yes', 'Cancel'],
        btn1: function (index) {
            $.ajax({
                url: "/adcomputer/ADComputerUnlock",
                type: "post",
                data: { name: name, __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() },
                dataType: "text",
                success: function (data) {
                    layer.msg(data);
                }
            });
            layer.close(index);
        },
        btn2: function (index) {
            layer.close(index)
        }
    });
}