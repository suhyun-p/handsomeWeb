$(window).load(function () {
    // realFeedbackAnalyze.init();
});

function realFeedbackAnalyze() { }

realFeedbackAnalyze.init = function () {
    $('#FeedbackGo').on('click', function () {

        var feedback = $('#targetFeedback').val();
        var param = [
                { name: 'Name1', value: feedback }
        ];

        $.ajax({
            url: "Feedback/FeedbackParsing",
            data: param,
            type: 'POST',
            dataType: "json",
            async: false,
            cache: false,
            success: function (data) {
                alert(data);
            },
            error: function (result) {
                alert("Fail");
            }
        });
    });

    $('#MongoTest').on('click', function () {
        var feedback = $('#targetFeedback').val();
        var param = [
                { name: 'Name1', value: feedback }
        ];

        $.ajax({
            url: "Feedback/MongoDBTest",
            data: param,
            type: 'POST',
            dataType: "json",
            async: false,
            cache: false,
            success: function (data) {
                $.each(data, function (key, value) {
                    $('#myTable > tbody:last').append('<tr><td>' + key + '</td><td>' + value.Title + '</td><td>' + value.Content + '</td></tr>')
                });
            },
            error: function (result) {
                alert("Fail");
            }
        });
    });

    //$('#searchFeedbackReal').on('click', function () {
    //// $('#searchFeedbackReal').click(function(){
    //    var feedbackNo = $('#inputFeedbackNo').val();

    //    var param = [
    //        { name: 'orderno', value: feedbackNo }
    //    ];

    //    $.ajax({
    //        url: "../Feedback/GetRealFeedback",
    //        data: param,
    //        type: 'POST',
    //        dataType: "json",
    //        async: false,
    //        cache: false,
    //        success: function (data) {
    //            var a = 1;
    //            alert(112);
    //            // $("#realFeedbackTtile").value(data);
    //            // $(".caption > h3").html("TesT")
    //        },
    //        error: function (result) {
    //            alert("Fail");
    //        }
    //    });
    //});
}

function feedbackEvent() { }
feedbackEvent.searchFeedbackReal = function () {
    // $(".caption > h3").html("TesT");
    // $('#realFeedback').append('<div class="col-sm-6 col-md-4"><div class="thumbnail"><img src="http://img.iacstatic.co.kr/feedback/A/2016/12/5/f1098f87f26e45be8792297ebee166f7.jpg" alt="..."><div class="caption"><h3>좋아요~~</h3><p>가격만족도/성능/사용편의성 등 : 가격도 저렴하고 좋아요^^ 상태/배송 : 배송도 빠릅니다~^^ 그 외 도움이 될 만한 사용후기 : 항상 핑크만 썼었는데 이번에 노란색 주문해서 썼어요~ 노란색 향도 좋아요~</p><p><a href="#" class="btn btn-primary" role="button">Button</a> <a href="#" class="btn btn-default" role="button">Button</a></p></div></div></div></div>');

    var feedbackNo = $('#inputFeedbackNo').val();

        var param = [
            { name: 'orderno', value: feedbackNo }
        ];

        $.ajax({
            url: "../Feedback/GetRealFeedback",
            data: param,
            type: 'POST',
            dataType: "json",
            async: false,
            cache: false,
            success: function (data) {
                var a = 1;
                alert(112);
                // $("#realFeedbackTtile").value(data);
                // $(".caption > h3").html("TesT")
            },
            error: function (result) {
                alert("Fail");
            }
        });     
};