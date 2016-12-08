$(window).load(function () {
    realFeedbackAnalyze.init();
});

function realFeedbackAnalyze() { }

realFeedbackAnalyze.init = function(){
    

    	
	$('#MongoTest5').on('click', function () {
		var feedback = $('#targetFeedback').val();
		var param = [
                { name: 'Name1', value: feedback }
		];

		$.ajax({
			url: "/FeedbackSearch/MongoDBTest5",
			data: param,
			type: 'POST',
			dataType: "json",
			async: true,
			cache: true,
			success: function (data) {
				$('#myTable > tbody:last').html('');
				$.each(data, function (key, value) {
					$('#myTable > tbody:last').append('<tr><td width=20px>' + value.OrderNo + '</td><td width=100px><img src=\'' + value.ImageUrl.ImageUrl1 + '\' width=100px height=100px /> </td><td width=200px>' + value.Title + '</td><td>' + value.Contents + '</td></tr>')
				});
			},
			error: function (result) {
				alert("Fail");
			}
		});
	});

	$('#MongoTest4').on('click', function () {
		var feedback = $('#targetFeedback').val();
		var param = [
                { name: 'Name1', value: feedback }
		];

		$.ajax({
			url: "/FeedbackSearch/MongoDBTest4",
			data: param,
			type: 'POST',
			dataType: "json",
			async: true,
			cache: true,
			success: function (data) {
				$('#myTable > tbody:last').html('');
				$.each(data, function (key, value) {
					$('#myTable > tbody:last').append('<tr><td width=20px>' + value.OrderNo + '</td><td width=100px><img src=\'' + value.ImageUrl.ImageUrl1 + '\' width=100px height=100px /> </td><td width=200px>' + value.Title + '</td><td>' + value.Contents + '</td></tr>')
				});
			},
			error: function (result) {
				alert("Fail");
			}
		});
	});

	$('#MongoTest3').on('click', function () {
		var feedback = $('#targetFeedback').val();
		var param = [
                { name: 'Name1', value: feedback }
		];

		$.ajax({
			url: "/FeedbackSearch/MongoDBTest3",
			data: param,
			type: 'POST',
			dataType: "json",
			async: true,
			cache: true,
			success: function (data) {
				$('#myTable > tbody:last').html('');
				$.each(data, function (key, value) {
					$('#myTable > tbody:last').append('<tr><td width=20px>' + value.OrderNo + '</td><td width=100px><img src=\'' + value.ImageUrl.ImageUrl1 + '\' width=100px height=100px /> </td><td width=200px>' + value.Title + '</td><td>' + value.Contents + '</td></tr>')
				});
			},
			error: function (result) {
				alert("Fail");
			}
		});
	});

	$('#MongoTest2').on('click', function () {
		var feedback = $('#targetFeedback').val();
		var param = [
                { name: 'Name1', value: feedback }
		];

		$.ajax({
			url: "/FeedbackSearch/MongoDBTest2",
			data: param,
			type: 'POST',
			dataType: "json",
			async: true,
			cache: true,
			success: function (data) {
				$('#myTable > tbody:last').html('');
				$.each(data, function (key, value) {
					$('#myTable > tbody:last').append('<tr><td width=20px>' + value.OrderNo + '</td><td width=100px><img src=\'' + value.ImageUrl.ImageUrl1 + '\' width=100px height=100px /> </td><td width=200px>' + value.Title + '</td><td>' + value.Contents + '</td></tr>')
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