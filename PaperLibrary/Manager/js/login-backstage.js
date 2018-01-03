$(function(){
	$(".login-container").height($(window).height());
	$(window).resize(function(){
		$(".login-container").height($(window).height());
	})
	$(".choice .jump-search").click(function(){
		console.log(2);
	})
	$(".verify").click(function () {
	    $(this).attr("src", "validate.aspx?" + new Date());

	});
})