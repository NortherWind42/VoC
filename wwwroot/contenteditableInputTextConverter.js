(function () {

	$(".voc_textarea").on("change keyup paste", function () {

		let htmlWithoutTooltip = $(this).html().replace(
			/((<div.*>).*(<\/div>))/g, function (match, value) {
				return "";
			}
		);

		$(this).html(htmlWithoutTooltip);

		let text = $(this).text();

		let directiveLine = 'data-toggle="tooltip" data-placement="top"';

		let html = text.replace(
			/(([^\u0000-\u007F]|\w)+'?(([^\u0000-\u007F]|\w)+)?)+/g, function (match, value) {
				return `<span ${directiveLine}>${value}</span>`;
			}
		);

		let clearHtml = html.replace(
			/(\<[span]{4}(\s+)([A-Za-z0-9-\=\ \"]*)\>)(\s*)(\<\/[span]{4}\>)/gm, function (match, value) {
				return "";
			}
		);

		$(this).html(clearHtml);


		(function () {
			$('[data-toggle="tooltip"]').mouseenter(function () {
				let $that = $(this);
				let content = $that["0"].textContent;


				$that.attr('data-original-title', content);
			});
		})();

		$(function () {
			$('[data-toggle="tooltip"]').tooltip()
		})

		let textarea = document.getElementsByClassName("voc_textarea")[0];
		cursorManager.setEndOfContenteditable(textarea);

	});

})();

