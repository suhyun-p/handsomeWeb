$(document).ready(function () {
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
});