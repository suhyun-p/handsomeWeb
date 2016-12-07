﻿$(window).load(function () {
    realFeedbackAnalyze.init();
});

function realFeedbackAnalyze() { }

realFeedbackAnalyze.init = function(){
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

    // $('#searchFeedbackReal').on('click', function () {
    //$('#searchFeedbackReal').click(function(){
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
    //            // $("#realFeedbackTtile").value(data);
    //            $(".caption > h3").html("TesT")
    //        },
    //        error: function (result) {
    //            alert("Fail");
    //        }
    //    });
    //});
}

function feedbackEvent() { }
feedbackEvent.searchFeedbackReal = function () {
    $(".caption > h3").html("TesT")
};