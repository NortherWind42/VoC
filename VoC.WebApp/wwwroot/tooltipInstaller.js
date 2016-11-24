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
			data = { word: word}
			$.ajax({
			    url: '/api/Main/GetTranslation',
			    data: data,
			    headers: {
			        'Authorization': 'Bearer  ' + sessionStorage.getItem("token"),
			        'Content-Type': 'application/json'
			    },
			}).done(function (data) {
			    $(current.currentTarget).attr('data-original-title', data);

			    $(function () {
			        $(current.currentTarget).tooltip('show')
			    })
			});

			
		}
	)

};