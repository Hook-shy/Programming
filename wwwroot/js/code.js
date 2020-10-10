function File(type, filename, language, code, row, col, id, position) {
    this.type = type;
    this.filename = filename;
    this.language = language;
    this.code = code;
    this.row = row;
    this.col = col;
    this.id = id;
    this.position = position;
}

var codeeditorattribute = { theme: 'vs', editortype: 'monacoeditor' };

var codeEditor = { openfiles: [], allfiles: [], openfileid: 0 };

function Lang(name, extname, text, startcode) {
    this.name = name;
    this.extname = extname;
    this.text = text;
    this.startcode = startcode
}

var languageset = [
    new Lang('c', 'c', 'C语言', '#include <stdio.h>\n\nint main() {\n    printf("Hello World !");\n    return 0;\n}'),
    new Lang('cpp', 'cpp', 'C++', '#include <iostream>\nint main()\n{\n    using namespace std;\n    cout << "Hello World\\n";\n    return 0;\n}'),
    new Lang('csharp', 'cs', 'C#', 'using System;\n\nnamespace ConsoleApplication\n{\n    class Program\n    {\n        static void Main(string[] args)\n        {\n            Console.WriteLine("hello world");\n            Console.ReadLine();\n        }\n    }\n}'),
    new Lang('css', 'css', 'CSS', 'body {\n    color: #66ccff\n}'),
    new Lang('html', 'html', 'HTML', '<!DOCTYPE html>\n\n<html>\n\n<head>\n    <meta charset="UTF-8">\n    <title>Hello World</title>\n</head>\n\n<body>\n    Hello World\n</body>\n\n</html>'),
    new Lang('java', 'java', 'Java', 'public class Main {\n    public static void main(String[] args) {\n        System.out.println("Hello World");\n    }\n}'),
    new Lang('javascript', 'js', 'JavaScript', 'console.log("Hello World");'),
    new Lang('json', 'json', 'json', '{\n    "word":"Hello World"\n}'),
    new Lang('php', 'php', 'PHP', '<?php\n    echo "Hello World!\\n" ;\n?>'),
    new Lang('python', 'py', 'Python', 'print("Hello World")'),
    new Lang('txt', 'txt', '纯文本', 'Hello World'),
    new Lang('xml', 'xml', 'XML', ' '),
]

function refreshlanlist() {
    var menuitemlanlist = document.getElementById('menuitemlanlist');
    var newfileboxselect = document.getElementById('newfileboxselect');
    var uploadfile = document.getElementById('uploadfile');
    menuitemlanlist.innerHTML = ' ';
    newfileboxselect.innerHTML = ' ';
    uploadfile.accept = ' ';
    for (var i = 0; i < languageset.length; i++) {
        menuitemlanlist.innerHTML += '<li onclick="setLanguage(\'' + languageset[i].name + '\')">' + languageset[i].text + ' (' + languageset[i].extname + ')</li>';
        newfileboxselect.innerHTML += '<option value="' + languageset[i].name + '">' + languageset[i].text + ' (' + languageset[i].extname + ')</option>';
        uploadfile.accept += ',.' + languageset[i].extname;
    }
}

refreshlanlist();

function getID() {
    var r = 1;
    for (var i = 0; i < codeEditor.allfiles.length; i++) {
        if (codeEditor.allfiles[i].id >= r) {
            r = codeEditor.allfiles[i].id;
        }
    }
    for (var i = 0; i < codeEditor.openfiles.length; i++) {
        if (codeEditor.openfiles[i].id >= r) {
            r = codeEditor.openfiles[i].id;
        }
    }
    return ++r;
}

codeEditor.openfiles.unshift(new File('startpage', '开始', null, null, null, null, getID()));
codeEditor.openfileid = codeEditor.openfiles[0].id;

function refreshfileitems() {
    document.getElementById('file').innerHTML = "";
    var openfileindex = 0;
    for (var i = 0; i < codeEditor.openfiles.length; i++) {
        var s = '<li class="fileitems" onclick="fileitemclick(' + codeEditor.openfiles[i].id + ')"><div class="fileitems ' + (codeEditor.openfiles[i].id == codeEditor.openfileid ? 'fileitemsactive' : '') + '">' + codeEditor.openfiles[i].filename + '<div class="myclose" title="关闭" onclick="fileitemclose(' + codeEditor.openfiles[i].id + ')">&#215;</div></div></li>';
        document.getElementById('file').innerHTML += s;
        if (codeEditor.openfiles[i].id == codeEditor.openfileid) {
            openfileindex = i
        }
    }
    var nowfile = codeEditor.openfiles[openfileindex];
    if (nowfile.type == 'startpage') {
        document.getElementById('startoperation').style.display = 'block';
        document.getElementById('editarea').style.display = 'none';
        document.getElementById('theme').innerHTML = '';
        document.getElementById('language').innerHTML = '';
        document.getElementById('location').innerHTML = '';
    } else {
        document.getElementById('startoperation').style.display = 'none';
        document.getElementById('editarea').style.display = 'block';
        document.getElementById('theme').innerHTML = codeeditorattribute.theme;
        for (var i = 0; i < languageset.length; i++) {
            if (nowfile.language == languageset[i].name) {
                document.getElementById('language').innerHTML = languageset[i].text;
                break;
            }
        }
        document.getElementById('location').innerHTML = '行 ' + nowfile.row;
        document.getElementById('location').innerHTML += ',列 ' + nowfile.col;
        monacoEditor && monacoEditor.dispose();
        document.getElementById('editor').innerHTML = ' ';
        monacoEditor = monaco.editor.create(document.getElementById('editor'), {
            value: nowfile.code,
            language: nowfile.language,
            theme: codeeditorattribute.theme,
            automaticLayout: true,
            smoothScrolling: true,
        });
        monacoEditor.onDidChangeCursorPosition((e) => {
            ChangeCursorPosition(e);
        });
        monacoEditor.onDidChangeModelContent((e) => {
            ChangeModelContent(e)
        });
        monacoEditor.focus();
        nowfile.position && monacoEditor.setPosition(nowfile.position);
        // monacoEditor.setValue(nowfile.code);
        // monaco.editor.setModelLanguage(monaco.editor.getModel(), nowfile.language);
    }
}

function fileitemclick(id) {
    for (var i = 0; i < codeEditor.openfiles.length; i++) {
        if (codeEditor.openfiles[i].id == id) {
            if (codeEditor.openfiles == id) {
                return;
            }
            codeEditor.openfileid = id;
            refreshfileitems();
            return;
        }
    }
}

function fileitemclose(id) {
    var index = -1;
    for (var i = 0; i < codeEditor.openfiles.length; i++) {
        if (id == codeEditor.openfiles[i].id) {
            index = i;
            break;
        }
    }
    if (index > -1) {
        codeEditor.openfileid = codeEditor.openfiles[index].id == id ? (codeEditor.openfiles[index - 1] && codeEditor.openfiles[index - 1].id) || (codeEditor.openfiles[index + 1] && codeEditor.openfiles[index + 1].id) : codeEditor;
        codeEditor.openfiles.splice(index, 1);
        if (codeEditor.openfiles.length <= 0) {
            codeEditor.openfiles.unshift(new File('startpage', '开始', null, null, null, null, getID()));
            codeEditor.openfileid = codeEditor.openfiles[0].id;
        }
        refreshfileitems();
        refreshfilelist();
    }
}

function refreshfilelist() {
    codeEditor.openfiles.sort(function (a, b) { return b.filename - a.filename });
    codeEditor.allfiles.sort(function (a, b) { return b.filename - a.filename });
    var openfileslist = document.getElementById('openfileslist');
    openfileslist.innerHTML = ' ';
    for (var i = 0; i < codeEditor.openfiles.length; i++) {
        if (codeEditor.openfiles[i].type == 'code') {
            openfileslist.innerHTML += '<li onclick="filelistclick(' + codeEditor.openfiles[i].id + ')">' + codeEditor.openfiles[i].filename + '</li>';
        }
    }
    var allfileslist = document.getElementById('allfileslist');
    allfileslist.innerHTML = ' ';
    for (var i = 0; i < codeEditor.allfiles.length; i++) {
        allfileslist.innerHTML += '<li onclick="filelistclick(' + codeEditor.allfiles[i].id + ')">' + codeEditor.allfiles[i].filename + '</li>';
    }
}

function filelistclick(id) {
    var flag = false;
    for (var i = 0; i < codeEditor.openfiles.length; i++) {
        if (id == codeEditor.openfiles[i].id) {
            flag = true;
            break;
        }
    }
    if (!flag) {
        var index = 0;
        for (var i = 0; i < codeEditor.allfiles.length; i++) {
            if (id == codeEditor.allfiles[i].id) {
                index = i;
                break;
            }
        }
        codeEditor.openfiles.unshift(codeEditor.allfiles[index]);
    }
    fileitemclick(id);
    refreshfilelist();
}

refreshfileitems();
refreshfilelist();

var activemenu = '';

function menu_MouseOver(menuitem) {
    document.getElementById(menuitem).style.borderLeft = '3px solid white';
    document.getElementById(menuitem).style.backgroundColor = 'rgb(0, 255, 221)';
}

function menu_MouseOut(menuitem) {
    if (activemenu !== menuitem) {
        document.getElementById(menuitem).style.borderLeft = '3px solid transparent';
        document.getElementById(menuitem).style.backgroundColor = '#00B890';
    }
}

function menu_MouseClick(menuitem) {
    menuIdList = ['myMenuIconFile', 'myMenuIconEdit', 'myMenuIconRun', 'myMenuIconSetting'];
    for (var i = 0; i < menuIdList.length; i++) {
        if (menuIdList[i] != menuitem) {
            document.getElementById(menuIdList[i]).style.borderLeft = '3px solid transparent';
            document.getElementById(menuIdList[i]).style.backgroundColor = '#00B890';
        }
    }

    function hideMenuItems() {
        document.getElementById('myMenuItemFile').style.display = 'none';
        document.getElementById('myMenuItemEdit').style.display = 'none';
        document.getElementById('myMenuItemRun').style.display = 'none';
        document.getElementById('myMenuItemSetting').style.display = 'none';
    }
    activemenu = (activemenu === '' ? menuitem : (activemenu === menuitem ? '' : menuitem));
    if (activemenu == '') {
        $('#mymenus').animate({ width: '10px' }, function () {
            document.getElementById('mymenus').style.display = 'none';
            hideMenuItems();
        });
        $('#myright').animate({ left: '50px' }, { duration: '50' });
    } else {
        hideMenuItems();
        document.getElementById(menuitem.replace('Icon', 'Item')).style.display = 'block';
        document.getElementById('mymenus').style.display = 'block';
        $('#mymenus').animate({ width: '250px' }, { duration: '50' });
        $('#myright').animate({ left: '300px' }, { duration: '50' });
    }
}

document.getElementById('myMenuIconFile').addEventListener('mouseover', function () { menu_MouseOver('myMenuIconFile'); });
document.getElementById('myMenuIconFile').addEventListener('mouseout', function () { menu_MouseOut('myMenuIconFile'); });
document.getElementById('myMenuIconEdit').addEventListener('mouseover', function () { menu_MouseOver('myMenuIconEdit'); });
document.getElementById('myMenuIconEdit').addEventListener('mouseout', function () { menu_MouseOut('myMenuIconEdit'); });
document.getElementById('myMenuIconRun').addEventListener('mouseover', function () { menu_MouseOver('myMenuIconRun'); });
document.getElementById('myMenuIconRun').addEventListener('mouseout', function () { menu_MouseOut('myMenuIconRun'); });
document.getElementById('myMenuIconSetting').addEventListener('mouseover', function () { menu_MouseOver('myMenuIconSetting'); });
document.getElementById('myMenuIconSetting').addEventListener('mouseout', function () { menu_MouseOut('myMenuIconSetting'); });

document.getElementById('myMenuIconFile').addEventListener('click', function () { menu_MouseClick('myMenuIconFile'); });
document.getElementById('myMenuIconEdit').addEventListener('click', function () { menu_MouseClick('myMenuIconEdit'); });
document.getElementById('myMenuIconRun').addEventListener('click', function () { menu_MouseClick('myMenuIconRun'); });
document.getElementById('myMenuIconSetting').addEventListener('click', function () { menu_MouseClick('myMenuIconSetting'); });

function setTheme(themestr) {
    codeeditorattribute.theme = themestr;
    monaco.editor.setTheme(themestr);
}

function setLanguage(languagestr) {
    var openfileindex = getopenfileindex()[0];
    if (codeEditor.openfiles[openfileindex].type == 'code') {
        codeEditor.openfiles[openfileindex].language = languagestr;
        refreshfileitems();
    }
}

function setEditorType(editorstr) {
    console.log(editorstr);
}

function showNewFileBox() {
    document.getElementById('backgraoundwindow').style.display = 'block';
    document.getElementById('windowback').style.display = 'block';
    document.getElementById('newfileboxtextbox').value = 'untitled';
    checkfilename();
}

function hideNewFileBox() {
    document.getElementById('backgraoundwindow').style.display = 'none';
    document.getElementById('windowback').style.display = 'none';
}

function openFile() {
    menu_MouseOver('myMenuIconFile');
    menu_MouseClick('myMenuIconFile');
    menu_MouseOver('myMenuIconFile');
    menu_MouseOut('myMenuIconFile');
}

function checkfilename() {
    document.getElementById('errorinfo').innerHTML = ' ';
    var filename = document.getElementById('newfileboxtextbox').value;
    if (filename == '') {
        document.getElementById('errorinfo').innerHTML = '文件名不能为空！';
        return false;
    } else if (filename.match(/[\\/:*?"<>]/i)) {
        document.getElementById('errorinfo').innerHTML = '文件名中存在非法字符！';
        return false;
    }
    var filetype = document.getElementById('newfileboxselect').value;
    for (var i = 0; i < languageset.length; i++) {
        if (filetype == languageset[i].name) {
            filename += '.' + languageset[i].extname;
            break;
        }
    }
    for (var i = 0; i < codeEditor.allfiles.length; i++) {
        if (codeEditor.allfiles[i].filename == filename) {
            document.getElementById('errorinfo').innerHTML = '文件' + filename + '已存在！'
            return false;
        }
    }
    return { fn: filename, type: filetype };
}

function getstartcode(language) {
    for (var i = 0; i < languageset.length; i++) {
        if (language == languageset[i].name) {
            return languageset[i].startcode;
        }
    }
    return ' ';
}

function buttonokclick() {
    var f;
    if (f = checkfilename()) {
        createfile(new File('code', f.fn, f.type, getstartcode(f.type), 1, 1, getID()));
        hideNewFileBox();
    }
}

function importfile(input) {
    //支持chrome IE10 
    if (window.FileReader) {
        function readfile(index) {
            if (input.files[index]) {
                var filename = input.files[index].name.toString();
                var fileexist = false;
                for (var i = 0; i < codeEditor.allfiles.length; i++) {
                    if (filename == codeEditor.allfiles[i].filename) {
                        fileexist = true;
                        break;
                    }
                }
                if (!fileexist) {
                    var filetype = 'txt';
                    for (var i = 0; i < languageset.length; i++) {
                        if (languageset[i].extname == filename.substr(filename.lastIndexOf(".") + 1)) {
                            filetype = languageset[i].name;
                            break;
                        }
                    }
                    var reader = new FileReader();
                    reader.readAsText(input.files[index], 'utf-8');
                    reader.onload = function () {
                        createfile(new File('code', filename, filetype, this.result, 1, 1, getID()));
                        readfile(++index);
                    }
                }
            }
        }
        readfile(0);
    } else {
        alert('文件读取错误！');
    }
}

function exportfile() {
    var openfileindex = getopenfileindex();
    if (openfileindex[0] >= 0 && codeEditor.openfiles[openfileindex[0]].type == 'code') {
        savefiletolocation(codeEditor.allfiles[openfileindex[1]].filename, codeEditor.allfiles[openfileindex[1]].code);
    }
}

function savefiletolocation(filename, content) {
    function fake_click(obj) {
        var ev = document.createEvent("MouseEvents");
        ev.initMouseEvent(
            "click", true, false, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null
        );
        obj.dispatchEvent(ev);
    }

    function download(name, data) {
        var urlObject = window.URL || window.webkitURL || window;
        var downloadData = new Blob([data]);
        var save_link = document.createElementNS("http://www.w3.org/1999/xhtml", "a");
        save_link.href = urlObject.createObjectURL(downloadData);
        save_link.download = name;
        fake_click(save_link);
    }
    download(filename, content);
}

function showandhideDebugdiv() {
    if (document.getElementById('debug').style.display != 'block') {
        document.getElementById('debug').style.display = 'block';
        $('#debug').animate({ width: '50%' }, { duration: '50' });
        $('#editor').animate({ width: '50%' }, { duration: '50' });
    } else {
        $('#debug').animate({ width: '0' }, function () {
            document.getElementById('debug').style.display = 'none';
        });
        $('#editor').animate({ width: '100%' }, { duration: '50' });
    }
}

function showdebugdiv() {
    if (document.getElementById('debug').style.display != 'block') {
        document.getElementById('debug').style.display = 'block';
        $('#debug').animate({ width: '50%' }, { duration: '50' });
        $('#editor').animate({ width: '50%' }, { duration: '50' });
    }
}

function hidedebugdiv() {
    if (document.getElementById('debug').style.display != 'none') {
        $('#debug').animate({ width: '0' }, function () {
            document.getElementById('debug').style.display = 'none';
        });
        $('#editor').animate({ width: '100%' }, { duration: '50' });
    }
}

function createfile(file) {
    codeEditor.allfiles.unshift(file);
    codeEditor.openfiles.unshift(file);
    fileitemclick(file.id);
    refreshfilelist();
}

function getopenfileindex() {
    var openfileindexino = -1;
    for (var i = 0; i < codeEditor.openfiles.length; i++) {
        if (codeEditor.openfiles[i].id == codeEditor.openfileid) {
            openfileindexino = i;
            break;
        }
    }
    var openfileindexina = -1;
    for (var i = 0; i < codeEditor.allfiles.length; i++) {
        if (codeEditor.allfiles[i].id == codeEditor.openfileid) {
            openfileindexina = i;
            break;
        }
    }
    return [openfileindexino, openfileindexina];
}

function deletecurrentfile() {
    var openfileindex = getopenfileindex();
    if (openfileindex[0] >= 0 && codeEditor.openfiles[openfileindex[0]].type == 'code') {
        if (confirm('确定要删除文件"' + codeEditor.openfiles[openfileindex[0]].filename + '"吗？')) {
            fileitemclose(codeEditor.openfiles[openfileindex[0]].id);
            codeEditor.allfiles.splice(openfileindex[1], 1);
            refreshfilelist();
        }
    }
}

function renamecurrentfile() {
    var openfileindex = getopenfileindex();
    if (openfileindex[0] >= 0 && codeEditor.openfiles[openfileindex[0]].type == 'code') {
        var newname;
        if (newname = prompt('重命名', codeEditor.openfiles[openfileindex[0]].filename)) {
            for (var i = 0; i < codeEditor.allfiles.length; i++) {
                if (newname == codeEditor.allfiles[i].filename) {
                    alert('文件"' + newname + '"已存在！');
                    renamecurrentfile();
                    return
                }
            }
            codeEditor.allfiles[openfileindex[1]].filename = newname;
            refreshfilelist();
            refreshfileitems();
        }
    }
}

function ChangeModelContent(e) {
    codeEditor.openfiles[getopenfileindex()[0]].code = monacoEditor.getValue();
    codeEditor.openfiles[getopenfileindex()[0]].language == 'html' && ((document.getElementById('htmldebug').contentDocument || document.getElementById('htmldebug').contentWindow.document).children[0].innerHTML = monacoEditor.getValue());
}

function ChangeCursorPosition(e) {
    document.getElementById('location').innerHTML = '行 ' + e.position.lineNumber;
    document.getElementById('location').innerHTML += ',列 ' + e.position.column;
    codeEditor.openfiles[getopenfileindex()[0]].row = e.position.lineNumber;
    codeEditor.openfiles[getopenfileindex()[0]].col = e.position.column;
    codeEditor.openfiles[getopenfileindex()[0]].position = e.position;
}

function startdebug() {
    if (codeEditor.openfiles[getopenfileindex()[0]].type == "code") {
        var lang = codeEditor.openfiles[getopenfileindex()[0]].language;
        switch (lang) {
            case 'html':
                document.getElementById('htmldebug').style.display = 'block';
                document.getElementById('debugtextarea').style.display = 'none';
                ChangeModelContent();
                break;
            case 'c':
            case 'cpp':
            case 'java':
            case 'php':
                runcode(lang, monacoEditor.getValue());
                break;
            case 'csharp':
                runcode('cs', monacoEditor.getValue());
                break;
            case 'python':
                runcode('python3', monacoEditor.getValue());
                break;
            default:
                document.getElementById('htmldebug').style.display = 'none';
                document.getElementById('debugtextarea').style.display = 'none';
                break;
        }
        showdebugdiv();
    }
}

function clearconsole() {
    $('#debugtextarea').html('');
}

function runcode(lang, code) {
    var data = { lang: lang, language: lang, code: code, classname: '' };
    console.log(data);
    $.ajax({
        type: "POST",
        url: '/api/code/Run',
        contentType: "application/json;charset=UTF-8",
        data: JSON.stringify(data),
        success: (result) => {
            if (result.status == 'ok') {
                var runResult = JSON.parse(result.data.result);
                console.log(runResult);
                var out = $('#debugtextarea').html();
                if (runResult.message.compilation.success) {
                    out += '<span style="color:green">[文件] ' + codeEditor.openfiles[getopenfileindex()[0]].filename + ' ' + runResult.message.testcases[0].result + '</span>'
                    out += '<span style="color:green"> [内存] ' + runResult.message.testcases[0].memory + 'Byte</span>'
                    out += '<span style="color:green"> [时间] ' + runResult.message.testcases[0].time + 's</span><br>'
                    console.log(runResult);
                    var lines = runResult.message.testcases[0].stdout.split('\n');
                    for (var i = 0; i < lines.length; i++) {
                        out += exchangechar(lines[i]) + '<br>';
                    }
                } else {
                    out += '<span style="color:red">[文件] ' + codeEditor.openfiles[getopenfileindex()[0]].filename + '</span><br>'
                    var lines = runResult.message.compilation.log.split('\n');
                    for (var i = 0; i < lines.length; i++) {
                        out += '<span style="color:red">' + exchangechar(lines[i]) + '</span><br>';
                    }
                }
                $('#debugtextarea').html(out);
                document.getElementById('debugtextarea').style.height = '100%';
            }
        },
    });
}

function exchangechar(str) {
    var s = "";
    if (str.length == 0) return "";
    s = str.replace(/&/g, "&amp;");
    s = s.replace(/</g, "&lt;");
    s = s.replace(/>/g, "&gt;");
    s = s.replace(/ /g, "&nbsp;");
    return s;
}

function command(s) {
    monacoEditor.trigger('anyString', s);
}

// console.log(typeof monacoEditor);
// console.log(monacoEditor.getPosition());
// monacoEditor.onDidChangeCursorPosition((e) => {
//     console.log(e);
// });
// monacoEditor.onDidChangeModelContent((e) => {
//     console.log(e)
// });
// monacoEditor.updateOptions({
//     'theme': 'vs-dark',
// });
// var position = editor.getPosition();
// editor.executeEdits('', [{
//     range: {
//         startLineNumber: position.lineNumber,
//         startColumn: position.column,
//         endLineNumber: position.lineNumber,
//         endColumn: position.column
//     },
//     text: 'test'
// }]);
// monacoEditor.on('change', function(editor, change) {
//     render();
//     console.log(editor);
//     console.log(change);
// });
// monacoEditor.onDidChangeModelContent((e) => {
//     ChangeModelContent(e);
// })
// monacoEditor.onDidBlurEditor((e) => {
//     BlurEditor(e);
// })