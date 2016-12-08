$(window).load(function () {
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

    var frameSrc = "http://member.auction.co.kr/Feedback/VIPFeedbackSecondList.aspx?sellerid=n760527&itemno=A585443471&categoryCode=51190500";

    $('#myTabs').click('show', function (e) {
    	paneID = $(e.target).attr('href');
    	src = $(paneID).attr('data-src');
    	// if the iframe hasn't already been loaded once
    	if ($(paneID + " iframe").attr("src") == "") {
    		$(paneID + " iframe").attr("src", src);
    	}
    });



    	
    $('#MongoTest').on('click', function () {
        var feedback = $('#targetFeedback').val();
        var param = [
                { name: 'Name1', value: feedback }
        ];

        $.ajax({
        	url: "FeedbackSearch/MongoDBTest",
            data: param,
            type: 'POST',
            dataType: "json",
            async: true,
            cache: true,
            success: function (data) {
                $.each(data, function (key, value) {
                    $('#myTable > tbody:last').append('<tr><td>' + key + '</td><td>' + value.Title + '</td><td>' + value.Contents + '</td></tr>')
                });
            },
            error: function (result) {
                alert("Fail");
            }
        });
    });

    
}

function feedbackEvent() { }
feedbackEvent.searchFeedbackReal = function () {
    $(".caption > h3").html("TesT")
};