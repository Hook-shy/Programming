var userinfo = {};
var questions = [];
var r_match;
var questionsorder = 0;

(function() {
    $.ajax({
        type: 'POST',
        url: '../pys/r_get_user_info.py',
        success: function(result, status) {
            if (result.status == 'ok') {
                userinfo = result;
                $('#userinfo').text(' (' + userinfo.name + ')');
                requestquestions();
            }
        },
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

function requestquestions() {
    $.ajax({
        type: 'POST',
        url: '../pys/r_get_match.py',
        success: function(result, status) {
            if (result.status == 'ok') {
                // console.log(result);
                r_match = result;
                questions = result.questions;
                if (getdonecount() >= questions.length) {
                    showquestion(questions[questions.length - 1].flag, questions.length - 1);
                } else {
                    showquestion(questions[getdonecount()].flag, getdonecount());
                }
                $.ajax({
                    type: 'POST',
                    url: '../pys/r_get_match_time.py',
                    success: function(result, status) {
                        if (result.status == 'ok') {
                            userinfo.time = result.time;
                            setInterval(function() {
                                $.ajax({
                                    type: 'POST',
                                    url: '../pys/r_get_match_time.py',
                                    success: function(result, status) {
                                        if (result.status == 'ok') {
                                            userinfo.time = result.time;
                                            if (result.finish) {
                                                alert('比赛已结束');
                                                window.location.replace('../index.html');
                                            }
                                        }
                                    },
                                });
                            }, 5000);
                        }
                    }
                });
                setInterval(function() {
                    $('#time').text('剩余时间：' + exchangetime(r_match.total_time - (++userinfo.time)));
                    if (r_match.total_time - userinfo.time === 10) {
                        showtips('距离比赛结束只有 10s 了！');
                    }
                    if (r_match.total_time - userinfo.time === 0) {
                        showtips('时间到了，自动提交答案中...')
                        var ans = { questions: [] };
                        for (var i = 0; i < questions.length; i++) {
                            ans.questions.push({ id: r_match.id, qid: questions[i].id, flag: questions[i].flag })
                        }
                        // console.log(ans);
                        $.ajax({
                            type: 'POST',
                            url: '../pys/r_end_match.py',
                            data: { 'data': JSON.stringify(ans) },
                            success: function(result, status) {
                                if (result.status == 'ok') {
                                    showtips('答案提交成功')
                                    setTimeout(function() { window.location.replace('../index.html') }, 1000);

                                }
                            },
                        });
                    }
                }, 1000);
                $('#next_question').click(function() { nextquestion() });
                $('#back_question').click(function() { backquestion() });

            } else {
                showtips(result.msg);
            }
        }
    });
}


function showquestion(ans, index) {
    questionsorder = index;
    var s = ''
    if (questions[index].type == '单选题') {
        $('#question_title').text((index + 1) + '/' + questions.length + ' [单选题] ' + questions[index].title);
        var order = ['A. ', 'B. ', 'C. ', 'D. ', 'E. ', 'F. ', 'G. ']
        for (var i = 0; i < questions[index].options.length; i++) {
            if (ans == -1 || ans != i) {
                s += '<a href="#" class="list-group-item list-group-item-action mt-3" onclick="showquestion(' + i + ',' + index + ')">' + order[i] + questions[index].options[i] + '</a>';
            } else {
                s += '<a href="#" class="list-group-item list-group-item-action list-group-item-primary mt-3" onclick="showquestion(' + i + ',' + index + ')">' + order[i] + questions[index].options[i] + '</a>';
                if (questions[index].flag != i) {
                    questions[index].flag = i;
                    submitans({ questions: [{ id: r_match.id, qid: questions[index].id, flag: questions[index].flag }] });
                }
            }
        }
    } else if (questions[index].type == '多选题') {
        $('#question_title').text((index + 1) + '/' + questions.length + ' [多选题] ' + questions[index].title);
        var order = ['A. ', 'B. ', 'C. ', 'D. ', 'E. ', 'F. ', 'G. ']
        if (!(questions[index].flag instanceof Array)) {
            questions[index].flag = [];
        }

        if (ans != -1) {
            if (questions[index].flag.length > 1 && questions[index].flag.indexOf(ans) >= 0) {
                questions[index].flag.splice(questions[index].flag.indexOf(ans), 1);
                submitans({ questions: [{ id: r_match.id, qid: questions[index].id, flag: questions[index].flag }] });
            } else if (questions[index].flag.length == 1 && questions[index].flag[0] == ans) {

            } else {
                questions[index].flag.push(ans);
                submitans({ questions: [{ id: r_match.id, qid: questions[index].id, flag: questions[index].flag }] });
            }

        }
        for (var i = 0; i < questions[index].options.length; i++) {
            if (ans == -1 || questions[index].flag.indexOf(i) == -1) {
                s += '<a href="#" class="list-group-item list-group-item-action mt-3" onclick="showquestion(' + i + ',' + index + ')">' + order[i] + questions[index].options[i] + '</a>';
            } else {
                s += '<a href="#" class="list-group-item list-group-item-action list-group-item-primary mt-3" onclick="showquestion(' + i + ',' + index + ')">' + order[i] + questions[index].options[i] + '</a>';
            }
        }
    } else if (questions[index].type == '判断题') {
        $('#question_title').text((index + 1) + '/' + questions.length + ' [判断题] ' + questions[index].title);
        if (ans == -1) {
            s += '<a href="#" class="list-group-item list-group-item-action mt-3" onclick="showquestion(' + true + ',' + index + ')">' + '对' + '</a>';
            s += '<a href="#" class="list-group-item list-group-item-action mt-3" onclick="showquestion(' + false + ',' + index + ')">' + '错' + '</a>';
        } else {
            if (ans) {
                s += '<a href="#" class="list-group-item list-group-item-action list-group-item-primary mt-3" onclick="showquestion(' + true + ',' + index + ')">' + '对' + '</a>';
                s += '<a href="#" class="list-group-item list-group-item-action mt-3" onclick="showquestion(' + false + ',' + index + ')">' + '错' + '</a>';
                if (questions[index].flag != true) {
                    questions[index].flag = true;
                    submitans({ questions: [{ id: r_match.id, qid: questions[index].id, flag: questions[index].flag }] });
                }
            } else {
                s += '<a href="#" class="list-group-item list-group-item-action mt-3" onclick="showquestion(' + true + ',' + index + ')">' + '对' + '</a>';
                s += '<a href="#" class="list-group-item list-group-item-action list-group-item-primary mt-3" onclick="showquestion(' + false + ',' + index + ')">' + '错' + '</a>';
                if (questions[index].flag != false) {
                    questions[index].flag = false;
                    submitans({ questions: [{ id: r_match.id, qid: questions[index].id, flag: questions[index].flag }] });
                }
            }
        }
    } else if (questions[index].type == '填空题') {
        $('#question_title').html((index + 1) + '/' + questions.length + ' [填空题] ' + questions[index].title + '<br><br>');
        if (ans == -1) {
            s = '<input type="text" class="form-control" placeholder="输入答案" aria-label="Recipient\'s username" aria-describedby="basic-addon2" id="ans_input" onchange="ansinput()">'
        } else {
            s = '<input type="text" class="form-control" placeholder="输入答案" aria-label="Recipient\'s username" aria-describedby="basic-addon2" id="ans_input" onchange="ansinput()" value="' + ans.toString() + '">'
        }
    }
    $('#question_options').html(s);
    if (questionsorder == questions.length - 1) {
        $('#next_question').text('交卷');
    } else {
        $('#next_question').text('下一题');
    }
    $('#m_progress').attr('aria-valuenow', parseInt(getdonecount() * 100 / questions.length).toString());
    $('#m_progress').width(parseInt(getdonecount() * 100 / questions.length).toString() + '%');
}

function getdonecount() {
    var count = 0;
    for (var i = 0; i < questions.length; i++) {
        if (!(questions[i].flag !== -1)) {
            break;
        }
        count++;
    }
    return count;
}

function showtips(msg) {
    $('#tipscontent').text(msg);
    $('#tipswindow').toast('show');
    setTimeout(function() { $('#tipswindow').toast('hide'); }, 5000);
}

function submitans(ans) {
    $.ajax({
        type: 'POST',
        url: '../pys/r_submit_ans.py',
        data: { 'data': JSON.stringify(ans) },
        success: function(result, status) {
            if (result.status == 'ok') {
                // console.log(result);
                // console.log("提交成功");
            }
        },
    });
}

function finish() {
    if (getdonecount() >= questions.length) {
        if (confirm("确定交卷吗？") == true) {
            var ans = { questions: [] };
            for (var i = 0; i < questions.length; i++) {
                ans.questions.push({ id: r_match.id, qid: questions[i].id, flag: questions[i].flag })
            }
            // console.log(ans);
            $.ajax({
                type: 'POST',
                url: '../pys/r_end_match.py',
                data: { 'data': JSON.stringify(ans) },
                success: function(result, status) {
                    if (result.status == 'ok') {
                        showtips('答案提交成功！');
                        setTimeout(function() { window.location.replace('../index.html') }, 1000);
                    }
                },
            });
        }
    }
}

function ansinput() {
    questions[questionsorder].flag = $('#ans_input').val().toString();
    if (questions[questionsorder].flag.toString().length > 0) {
        submitans({ questions: [{ id: r_match.id, qid: questions[questionsorder].id, flag: questions[questionsorder].flag.toString().replace('\'', '').replace('\\', '').replace("\"", '').replace("'", '').replace('"', '') }] });
    }
}

function nextquestion() {
    if ($('#next_question').text().toString() === '交卷') {
        finish();
    }
    if (questionsorder < questions.length - 1) {
        // console.log(questions[questionsorder].flag);

        if (!(questions[questionsorder].flag == -1 || questions[questionsorder].flag.length === 0 || questions[questionsorder].flag.toString().length === 0)) {
            showquestion(questions[questionsorder + 1].flag, questionsorder + 1);
        }
    }
}

function backquestion() {
    if (questionsorder != 0) {
        showquestion(questions[questionsorder - 1].flag, questionsorder - 1);
    }
}