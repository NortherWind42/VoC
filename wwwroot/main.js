(function () {

	$(".voc_textarea").on("change keyup paste", function () {

		convertText(this);

		installTooltip();

		(function () {
			$(function () {
				$('[data-toggle="tooltip"]').tooltip()
			})
		})();

		cursorManager.setEndOfContenteditable($(this)[0]);
	});

})();

