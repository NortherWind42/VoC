(function () {

	$(".voc_textarea").on("change keyup paste", function () {

		convertText(this);

		installTooltip();

		cursorManager.setEndOfContenteditable($(this)[0]);
		
	});

})();

