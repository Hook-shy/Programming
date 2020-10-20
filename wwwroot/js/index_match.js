var r_index;
var userinfo;
var time;

$(function() {
    // 1. 轮播图
    $(window).on('resize', () => {
        // 1.窗口的宽度
        let clientW = $(window).width();
        // 2. 设置临界点
        let isShowBigImage = clientW >= 900;
        // 3. 获取所有item
        let $allItems = $('#lk_carousel .carousel-item');
        // console.log($allItems);
        // 4. 遍历
        $allItems.each((index, item) => {
            // 4.1 取出图片的路径
            let src = isShowBigImage ? $(item).data('lg-img') : $(item).data('sm-img');
            let imgUrl = `url(${src})`;
            // 4.2 设置背景
            $(item).css({
                backgroundImage: imgUrl
            });
            // 4.3 创建img标签
            if (!isShowBigImage) { // 大屏幕
                let imgEle = `<img src="${src}">`;
                $(item).empty().append(imgEle);
            } else { // 小屏幕
                $(item).empty();
            }
        });
    });
    $(window).trigger('resize');

    // 3. 添加轮播图的滑动
    let startX = 0,
        endX = 0;
    let carouselInner = $('#lk_carousel .carousel-inner')[0];
    let $carousel = $('#lk_carousel');
    let carousel = $carousel[0];

    carouselInner.addEventListener('touchstart', (e) => {
        startX = e.targetTouches[0].clientX;
    });
    carouselInner.addEventListener('touchmove', (e) => {
        endX = e.targetTouches[0].clientX;
        if (endX - startX > 0) { // 上一张
            $carousel.carousel('prev');
        } else if (endX - startX < 0) { // 下一张
            $carousel.carousel('next');
        }
    });

    // 4. 超出内容处理
    $(window).on('resize', () => {
        let $ul = $('#lk_product .nav');
        let $allLis = $('.nav-item', $ul);
        let totalW = 0; // 所有li的宽度
        $allLis.each((index, item) => {
            totalW += $(item).width();
        });
        // console.log(totalW);

        // 获取父标签的宽度
        let parentW = $ul.parent().width();
        // console.log(parentW);
        if (totalW > parentW) {
            $ul.css({
                width: totalW + 'px'
            })
        } else {
            $ul.removeAttr('style');
        }
    }).trigger('resize');

    // 2. 工具的提示
    $('[data-toggle="tooltip"]').tooltip();
});

(function() {
    //模态框
    $('#myModal').on('shown.bs.modal', function() { $('#myInput').trigger('focus') })
        //导航栏滚动监听
    $('body').scrollspy({ target: '#navbar-example' })
        // $('#sign').click(function () { sign(); })
    $.ajax({
        type: 'POST',
        url: 'pys/r_index.py',
        success: function(result, status) {
            if (result.status == 'ok') {
                r_index = result;
                showmatchblock();
                // console.log(result);
                showclass();
                showranklist('total');
                $('#scq-count').text('【' + r_index.q_count.scq + '】');
                $('#mcq-count').text('【' + r_index.q_count.mcq + '】');
                $('#tfq-count').text('【' + r_index.q_count.tfq + '】');
                $('#cq-count').text('【' + r_index.q_count.cq + '】');
                $.ajax({
                    type: 'POST',
                    url: 'pys/r_get_user_info.py',
                    success: function(result, status) {
                        if (result.status == 'ok') {
                            userinfo = result;
                            // console.log(userinfo.name);

                            $('#signbutton').css('display', 'none');
                            $('#userimg').css('display', 'block');
                            $('#usernamediv').css('display', 'block');
                            $('#username').html(userinfo.name.toString());
                        } else {
                            $('#match-btn').text('报名参赛');
                        }
                    },
                });
            }
        },
    });
})();

function showmatchblock() {
    if (r_index.match) {
        $('#form_race').removeAttr('style');
    }
    if (r_index.practise) {
        $('#lk_hot').removeAttr('style');
    }
    if (r_index.match_info && r_index.match_info.count >= 1 && r_index.match_info.order > 0) {
        $('#my-score-block').removeAttr('style');
        $('#card-order').text(r_index.match_info.order);
        $('#card-score').text(r_index.match_info.score);
        $('#card-time').text(exchangetime(parseInt(r_index.match_info.time)));
        $('#card-count').text(r_index.match_info.count + '/' + r_index.times);
        if (!r_index.match_info.match_end) {
            $('#continue-match').removeAttr('style');
        }
    } else {
        $('#match-rule-block').removeAttr('style');
    }

}

function exchangetime(t) {
    var h, m, s;
    t = parseInt(t);
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


// 显示班级
function showclass() {
    var s = ''
    for (var i = 0; i < r_index.class.length; i++) {
        if (i == 0) {
            s += '<option selected value="' + r_index.class[i] + '">' + r_index.class[i] + '</option>';
        } else {
            s += '<option value="' + r_index.class[i] + '">' + r_index.class[i] + '</option>';
        }
    }
    $('#inlineFormCustomSelectPref').html(s);
}

// 显示排行榜
function showranklist(flag) {
    var ranklist = r_index.rank_list;
    var s = ''
    for (var i = 0; i < ranklist.practise.list.length; i++) {
        s += '<tr>';
        switch (i) {
            case 0:
                s += '<td scope="row"><img src="images/ph_icon_41.png" alt=""></td>';
                break;
            case 1:
                s += '<td scope="row"><img src="images/ph_icon_42.png" alt=""></td>';
                break;
            case 2:
                s += '<td scope="row"><img src="images/ph_icon_43.png" alt=""></td>';
                break;
            default:
                s += '<td scope="row">' + ranklist.practise.list[i].order + '</td>';
                break;
        }
        s += '<td class="align-middle">' + ranklist.practise.list[i].name + '</td>';
        s += '<td class="align-middle">' + ranklist.practise.list[i].p_count + '</td>';
        s += '<td class="align-middle">' + exchangetime(ranklist.practise.list[i].p_time) + '</td>';
        s += '</tr>';
    }
    $('#p_ranklist').html(s);
    s = ''
    for (var i = 0; i < ranklist.match.list.length; i++) {
        s += '<tr>';
        switch (i) {
            case 0:
                s += '<td scope="row"><img src="images/ph_icon_41.png" alt=""></td>';
                break;
            case 1:
                s += '<td scope="row"><img src="images/ph_icon_42.png" alt=""></td>';
                break;
            case 2:
                s += '<td scope="row"><img src="images/ph_icon_43.png" alt=""></td>';
                break;
            default:
                s += '<td scope="row">' + ranklist.match.list[i].order + '</td>';
                break;
        }
        s += '<td class="align-middle">' + ranklist.match.list[i].name + '</td>';
        s += '<td class="align-middle">' + ranklist.match.list[i].score + '</td>';
        s += '<td class="align-middle">' + exchangetime(ranklist.match.list[i].time) + '</td>';
        s += '<td class="align-middle">' + ranklist.match.list[i].count + '</td>';
        s += '</tr>';
    }
    $('#m_ranklist').html(s);
    s = ''
    for (var i = 0; i < ranklist.cla.list.length; i++) {
        s += '<tr>';
        switch (i) {
            case 0:
                s += '<td scope="row"><img src="images/ph_icon_41.png" alt=""></td>';
                break;
            case 1:
                s += '<td scope="row"><img src="images/ph_icon_42.png" alt=""></td>';
                break;
            case 2:
                s += '<td scope="row"><img src="images/ph_icon_43.png" alt=""></td>';
                break;
            default:
                s += '<td scope="row">' + ranklist.cla.list[i].order + '</td>';
                break;
        }
        s += '<td class="align-middle">' + ranklist.cla.list[i].cla + '</td>';
        s += '<td class="align-middle">' + ranklist.cla.list[i].count + '</td>';
        s += '<td class="align-middle">' + ranklist.cla.list[i].average.toFixed(2) + '</td>';
        s += '</tr>';
    }
    $('#c_ranklist').html(s);
    if (ranklist.practise.count > 0) {
        $('#p_ranklist_count').text('参与人数：' + ranklist.practise.count);
    } else {
        $('#p_ranklist_count').text('');
    }
    if (ranklist.match.count > 0) {
        $('#m_ranklist_count').text('参与人数：' + ranklist.match.count);
    } else {
        $('#m_ranklist_count').text('');
    }
    if (ranklist.cla.average > 0) {
        $('#c_ranklist_count').html('平均答题次数：' + ranklist.cla.avg_count.toFixed(2) + '<br>总平均分：' + ranklist.cla.average.toFixed(2));
    } else {
        $('#c_ranklist_count').html('');
    }
}

//退出登录
function logout() {
    $.ajax({
        type: 'POST',
        url: 'pys/r_logout.py',
        success: function(result, status) {
            // console.log('退出');
            window.location.replace('index.html');
        }
    });
}

//检查登录信息
function check() {
    var form = document.forms['form1'];
    var name = form['recipient-name'].value;
    var num = form['message-text'].value;
    var cla = form['inlineFormCustomSelectPref'].value
    // console.log(name);
    // console.log(num);
    // console.log(cla);
    if (name.length < 2 || name.length > 4) {
        alert('姓名至少为2个字符，最多为4个字符！');
        return false;
    }
    if (num.length !== 12) {
        alert('学号格式错误！');
        return false;
    }
    if (isNaN(parseInt(num))) {
        alert('学号格式错误！');
        return false;
    }
    return true;
}


//点击跳转
//单选题
document.getElementById("entrance1").onclick = function() {
    if (userinfo == null) {
        $('#exampleModal').modal('show')
        return;
    }
    window.location = "pages/practise_scq.html";
}

//多选题
document.getElementById("entrance2").onclick = function() {
    if (userinfo == null) {
        $('#exampleModal').modal('show')
        return;
    }
    window.location = "pages/practise_mcq.html";
}

//判断题
document.getElementById("entrance3").onclick = function() {
    if (userinfo == null) {
        $('#exampleModal').modal('show')
        return;
    }
    window.location = "pages/practise_tfq.html";
}

//填空题
document.getElementById("entrance4").onclick = function() {
    if (userinfo == null) {
        $('#exampleModal').modal('show')
        return;
    }
    window.location = "pages/practise_cq.html";
}

document.getElementById("match-btn").onclick = function() {
    if (userinfo == null) {
        $('#exampleModal').modal('show')
        return;
    }
    $('#exampleModalLong').modal('show')
}

//开始比赛
document.getElementById("start_match").onclick = function() {
    if (userinfo == null) {
        $('#exampleModal').modal('show')
        return;
    }
    window.location = "pages/match.html";
}
document.getElementById("continue-match").onclick = function() {
    if (userinfo == null) {
        $('#exampleModal').modal('show')
        return;
    }
    $('#exampleModalLong').modal('show')
}