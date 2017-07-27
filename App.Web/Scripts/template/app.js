$(document).ready(function(){

	/* Content appear */
	if($('body').hasClass('content-appear')) {
		$('body').addClass('content-appearing')
		setTimeout(function() {
			$('body').removeClass('content-appear content-appearing');
		}, 800);
	}
	
	/* Preloader */
	setTimeout(function() {
		$('.preloader').fadeOut();
	}, 500);

	/* Scroll */
	if(jQuery.browser.mobile == false) {
		function initScroll(){
			$('.custom-scroll').jScrollPane({
				autoReinitialise: true,
				autoReinitialiseDelay: 100
			});
		}

		initScroll();

		$(window).resize(function() {
			initScroll();
		});
	}

	/* Sidebar - if mobile */
	if(jQuery.browser.mobile == true) {
		$('body').removeClass('fixed');
	}

    /* Sidebar - on click */
	$('.large-sidebar .site-sidebar li.with-sub').each(function() {
		var li = $(this),
			clickLink = li.find('>a'),
			subMenu = li.find('>ul');

		clickLink.click(function(){
			if (li.hasClass('active')) {
				li.removeClass('active');
				subMenu.slideUp();
			} else {
				if (!li.parent().closest('.with-sub').length) {
					$('.site-sidebar li.with-sub').removeClass('active').find('>ul').slideUp();
				}
				li.addClass('active');
				subMenu.slideDown();
			}
		});
	});

	/* Sidebar - if active */
	function sidebarIfActive(){
		$('.site-sidebar li.with-sub').removeClass('active').find('>ul').hide();
		var url = window.location;
	    var element = $('.site-sidebar ul li ul li a').filter(function () {
	        return this.href == url || url.href.indexOf(this.href) == 0;
	    });
		element.parent().addClass('active');
		element.parent().parent().parent().addClass('active');

	    if(!$('body').hasClass('compact-sidebar')) {
			element.parent().parent().slideDown();
	    }
	}

	sidebarIfActive();

	/* Sidebar - show and hide */
	$('.site-header .collapse-button').click(function() {
		if ($('body').hasClass('site-sidebar-opened')) {
			$(this).removeClass('active');
			$('body').removeClass('site-sidebar-opened');
			if(jQuery.browser.mobile == false){
				$('html').css('overflow','auto');
			}
		} else {
			$(this).addClass('active');
			$('body').addClass('site-sidebar-opened');
			if(jQuery.browser.mobile == false){
				$('html').css('overflow','hidden');
			}
		}
		if ($('body').hasClass('compact-sidebar')) {
			$('body').removeClass('compact-sidebar').addClass('large-sidebar');
			if(jQuery.browser.mobile == false) {
				$('body').addClass('fixed-sidebar');
				sidebarIfActive();
			}
		}
	});

	/* Sidebar - overlay */
	$('.site-sidebar-overlay').click(function() {
		$('.site-header .collapse-button').removeClass('active');
		$('body').removeClass('site-sidebar-opened');
		if(jQuery.browser.mobile == false){
			$('html').css('overflow','auto');
		}
	});
	

	/* Switchery */
	$('.site-sidebar-second .js-switch').each(function() {
		new Switchery($(this)[0], $(this).data());
	});

});