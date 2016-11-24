var installTooltip = function () {

	let languages = {};
	languages['en'] = 'Английский';
	languages['es'] = 'Испанский';
	languages['pt'] = 'Португальский';
	languages['ru'] = 'Русский';
	languages['bg'] = 'Болгарский';

	let target = $('[data-toggle="tooltip"]');

	let mouseEnter = Rx.Observable.fromEvent(target, 'mouseenter');
	let mouseLeave = Rx.Observable.fromEvent(target, 'mouseleave');

	let entered = mouseEnter
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

			let languageLine = languages['en'];

			$(current.currentTarget).attr('data-original-title', languageLine);

			$(function () {
				$(current.currentTarget).tooltip('show')
			})
		}
	)

};