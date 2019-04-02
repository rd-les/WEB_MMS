"use strict";
$('#calendar').fullCalendar({
	header: {
		left: 'prev',
		center: 'title',
		right: 'next'
	},
	defaultDate: '2018-07-12',
	editable: true,
	droppable: true, // this allows things to be dropped onto the calendar
	drop: function() {
		// is the "remove after drop" checkbox checked?
		if ($('#drop-remove').is(':checked')) {
			// if so, remove the element from the "Draggable Events" list
			$(this).remove();
		}
	},
	eventLimit: true, // allow "more" link when too many events
	events: [
		{
            title: 'WO1802874',
			start: '2018-07-03',
			className: 'b-l b-2x b-greensea'
		},
		{
            title: 'WO1802874',
			start: '2018-07-09',
			end: '2018-07-12',
			className: 'bg-cyan'
		},
		{
			id: 999,
            title: 'WO1802874',
			start: '2018-07-16',
			className: 'b-l b-2x b-success'
		},
		{
            title: 'WO1802874',
			start: '2018-07-11',
			end: '2018-07-13',
			className: 'b-l b-2x b-primary'
		},
		{
            title: 'WO1802874',
            start: '2018-07-19',
            end: '2018-07-22',
			className: 'b-l b-2x b-lightred'
		},
		{
            title: 'WO1802874',
			start: '2018-07-12',
			className: 'b-l b-2x b-amethyst'
		},
		{
            title: 'WO1807804',
            start: '2018-07-16',
            end: '2018-07-17',
			className: 'b-l b-2x b-primary'
		},
		{
            title: 'WO1802874',
			url: 'http://google.com/',
			start: '2018-07-30',
			className: 'b-l b-2x b-greensea'
		}
	]
});

// Hide default header
//$('.fc-header').hide();



// Previous month action
$('#cal-prev').click(function(){
	$('#calendar').fullCalendar( 'prev' );
});

// Next month action
$('#cal-next').click(function(){
	$('#calendar').fullCalendar( 'next' );
});

// Change to month view
$('#change-view-month').click(function(){
	$('#calendar').fullCalendar('changeView', 'month');

	// safari fix
	$('#content .main').fadeOut(0, function() {
		setTimeout( function() {
			$('#content .main').css({'display':'table'});
		}, 0);
	});

});

// Change to week view
$('#change-view-week').click(function(){
	$('#calendar').fullCalendar( 'changeView', 'agendaWeek');

	// safari fix
	$('#content .main').fadeOut(0, function() {
		setTimeout( function() {
			$('#content .main').css({'display':'table'});
		}, 0);
	});

});

// Change to day view
$('#change-view-day').click(function(){
	$('#calendar').fullCalendar( 'changeView','agendaDay');

	// safari fix
	$('#content .main').fadeOut(0, function() {
		setTimeout( function() {
			$('#content .main').css({'display':'table'});
		}, 0);
	});

});

// Change to today view
$('#change-view-today').click(function(){
	$('#calendar').fullCalendar('today');
});

/* initialize the external events
 -----------------------------------------------------------------*/
$('#external-events .event-control').each(function() {

	// store data so the calendar knows to render an event upon drop
	$(this).data('event', {
		title: $.trim($(this).text()), // use the element's text as the event title
		stick: true // maintain when user navigates (see docs on the renderEvent method)
	});

	// make the event draggable using jQuery UI
	$(this).draggable({
		zIndex: 999,
		revert: true,      // will cause the event to go back to its
		revertDuration: 0  //  original position after the drag
	});

});

$('#external-events .event-control .event-remove').on('click', function(){
	$(this).parent().remove();
});

// Submitting new event form
$('#add-event').submit(function(e){
	e.preventDefault();
	var form = $(this);

	var newEvent = $('<div class="event-control p-10 mb-10">'+$('#event-title').val() +'<a class="pull-right text-muted event-remove"><i class="fa fa-trash-o"></i></a></div>');

	$('#external-events .event-control:last').after(newEvent);

	$('#external-events .event-control').each(function() {

		// store data so the calendar knows to render an event upon drop
		$(this).data('event', {
			title: $.trim($(this).text()), // use the element's text as the event title
			stick: true // maintain when user navigates (see docs on the renderEvent method)
		});

		// make the event draggable using jQuery UI
		$(this).draggable({
			zIndex: 999,
			revert: true,      // will cause the event to go back to its
			revertDuration: 0  //  original position after the drag
		});

	});

	$('#external-events .event-control .event-remove').on('click', function(){
		$(this).parent().remove();
	});

	form[0].reset();

	$('#cal-new-event').modal('hide');

});
