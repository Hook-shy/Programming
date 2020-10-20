var userinfo;
var nowquestion = [];
var nowquestionorder = 0;

(function () {
    $.ajax({
        type: 'POST',
        url: '../pys/r_get_user_info.py',
        success: function (result, status) {
            // console.log(result);
            if (result.status == 'ok') {
                userinfo = result;
                $('#userinfo').text(' (' + userinfo.name + ')');
                requestquestion();
                $.ajax({
                    type: 'POST',
                    url: '../pys/r_get_practise_time.py',
                    success: function (result, status) {
                        if (result.status == 'ok') {
                            userinfo.p_time = result.p_time;
                            setInterval(function () {
                                $.ajax({
                                    type: 'POST',
                                    url: '../pys/r_get_practise_time.py',
                                    success: function (result, status) {
                                        if (result.status == 'ok') {
                                            userinfo.p_time = result.p_time;
                                        }
                                    },
                                });
                            }, 10000);
                        }
                    },
                });

                setInterval(function () {
                    $('#p_time').text('练习总时长：' + exchangetime(++userinfo.p_time));
                }, 1000)
            } else {
                window.location.replace('../index.html');
            }
        },
        error: function () {
            window.location.replace('../index.html');
        }
    });
})();

function exchangetime(t) {
    var h, m, s;
    s = (t % 60).toString();
    m = ((t - s) / 60 % 60).toString();
    h = ((t - s - m * 60) / 60 / 60).toString();
    if (s.length < 2) {
        s = '0' + s;
    }
    if (m.length < 2) {
        m = '0' + m;
    }
    if (h.length < 2) {
        h = '0' + h;
    }
    return h + ':' + m + ':' + s;
}

function requestquestion() {
    $.ajax({
        type: 'POST',
        url: '../pys/r_get_practise_question.py',
        data: { type: '填空题' },
        success: function (result, status) {
            // console.log(result);
            if (result.status == 'ok') {
                userinfo.count = result.p_count;
                $('#p_count').text('练习总数：' + userinfo.count);
                var question = result.question;
                question.flag = -1;
                nowquestion.push(question);
                // console.log(nowquestion);
                showquestion(-1, nowquestion.length - 1);
                if (nowquestion.length == 1) {
                    $('#next_question').click(function () { nextquestion() });
                    $('#back_question').click(function () { backquestion() });
                    $('#showabout').click(function () { showabout() });
                    $('#submintfeedback').click(function () { feedback() });
                }
            } else {
                window.location.replace('../index.html');
            }
        },
        error: function () {
            window.location.replace('../index.html');
        }
    });
}

function showquestion(ans, index) {
    // console.log(nowquestionorder);
    nowquestionorder = index;
    $('#question_title').text((index + 1) + '/' + nowquestion.length + ' [填空题] ' + nowquestion[index].title);
    var s = ''
    $('#question_title').html((index + 1) + '/' + nowquestion.length + ' [填空题] ' + nowquestion[index].title + '<br><br>');
    if (ans == -1) {
        s = '<input type="text" class="form-control" placeholder="输入答案" aria-label="Recipient\'s username" aria-describedby="basic-addon2" id="ans_input" onchange="ansinput()">'
    } else {
        s = '<input type="text" class="form-control" placeholder="输入答案" aria-label="Recipient\'s username" aria-describedby="basic-addon2" id="ans_input" onchange="ansinput()" value="' + ans.toString() + '">'
    }
    // console.log(nowquestion);

    $('#question_options').html(s);
    s = '<span class="text-success">正确答案：' + nowquestion[index].ans + '</span><br><br>';
    s += '<span class="text-info">题目分类：</span>' + nowquestion[index].kind + '<br><br>';
    s += '<span class="text-info">题目解析：</span><br>' + nowquestion[index].about + '<br><br><br>';
    $('#resolving').html(s);
    $('#errq').text(nowquestion[index].title);
    $('#message-text').val('');
}

function ansinput() {
    nowquestion[nowquestionorder].flag = $('#ans_input').val();
}

function showabout() {
    if (nowquestion) {
        $('#collapseExample').collapse('show');
    }
}

function showtips(msg) {
    $('#tipscontent').text(msg);
    $('#tipswindow').toast('show');
    setTimeout(function () { $('#tipswindow').toast('hide'); }, 5000);
}

function feedback() {
    var msgs = $('#message-text').val().toString().replace(/\s*/g, "");
    if (msgs.length == 0) {
        alert('错误说明不能为空');
    } else {
        $('#exampleModal').modal('hide');
        var feed = { data: { qid: nowquestion[nowquestionorder].id, msg: msgs } }
        $.ajax({
            type: 'POST',
            url: '../pys/r_feedback.py',
            data: { 'data': JSON.stringify(feed) },
            success: function (result, status) {
                if (result.status == 'ok') {
                    showtips('反馈成功！');
                }
            }
        });
    }
}

function nextquestion() {
    if (nowquestionorder >= nowquestion.length - 1) {
        $('#collapseExample').collapse('hide');
        requestquestion();
    } else if (nowquestionorder + 1 < nowquestion.length) {
        showquestion(nowquestion[nowquestionorder + 1].flag, nowquestionorder + 1);
    }
}

function backquestion() {
    if (nowquestionorder != 0) {
        showquestion(nowquestion[nowquestionorder - 1].flag, nowquestionorder - 1);
    }
}