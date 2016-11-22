var installTooltip = function () {

	$('[data-toggle="tooltip"]').mouseenter(function () {
			let $that = $(this);
			let content = $that["0"].textContent;

			//место для запроса

			$that.attr('data-original-title', content);
	});

};