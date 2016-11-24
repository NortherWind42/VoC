var installTooltip = function () {

	let target = $('[data-toggle="tooltip"]');

	target.mouseenter(function () {
		$(this).attr('data-original-title', "Loading...");
	});

	$(function () {
		$('[data-toggle="tooltip"]').tooltip()
	})

	let mouseEnter = Rx.Observable.fromEvent(target, 'mouseenter');
	let mouseLeave = Rx.Observable.fromEvent(target, 'mouseleave');

	var entered = mouseEnter
		.flatMap(function (current) {
			return Rx.Observable
				.of(current)
				.delay(1500)
				.takeUntil(mouseLeave);
		});

	entered.subscribe(
		function (current) {

			let word = current.currentTarget.innerText;

			// >посылаем слово на бэк
			// >возвращаем язык
			// >профит

			$(current.currentTarget).attr('data-original-title', word);

			$(function () {
				$(current.currentTarget).tooltip('show')
			})
		}
	)

};