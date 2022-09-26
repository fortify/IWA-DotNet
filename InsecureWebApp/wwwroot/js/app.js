$(document).ready(function () {
    var keywords = GetURLParameter("keywords");
    if (keywords) {
        $("#keywords").val(keywords);
    }
    $("#search").submit(function (event) {
        event.preventDefault();
        const keywords = document.getElementById('keywords').value;
        const p = document.getElementById("search-query");
        p.innerHTML = keywords;
        $('#search-query').parent().toggleClass("d-none");
        $.get("/api/products", { keywords: keywords, limit: 10 }, function (response) {
            $('#data tr').remove();
            if (response.length > 0) {
                $.each(response, function (i, row) {
                    $('#data').append(
                        "<div class='row p-4'>" +
                        "<div class='col-2'><img class='img-thumbnail' width='100px' src='/assets/img/" + row.image + "'></img></div>" +
                        "<div class='col-2'><a href='/products/" + row.id + "'>" + row.name + "</a></div>" +
                        "<div class='col-4'>" + row.summary.substring(0, 40) + "&hellip;</div>" +
                        "<div class='col-1'>&#164;" + row.price + "</div>" +
                        "<div class='col-2'><input id='input-" + i + "' class='star-rating' name='input-" + i + "' value='" + row.rating + "'></div>" +
                        "<div class='col-1'>" +
                        (row.inStock ? "<span class='badge badge-success'>In Stock</span>" : "<span class='badge badge-danger'>Out of Stock</span>") +
                        "</div>" +
                        "</div>");
                });
                (function () {
                    setTimeout(function () {
                        $('input[class="star-rating"]').each(function () {
                            $(this).rating({ displayOnly: true, showCaption: false, step: 1.0, size: 'xs', theme: 'krajee-fas' });
                        })
                    }, 100);
                })();
            } else {
                $('#data').append("<tr><th>No results found</th></tr>")
            }
        });
    });

    window.setTimeout(function () {
        $(".alert").fadeTo(1000, 0).slideUp(1000, function () {
            $(this).remove();
        });
    }, 5000);

    // create site menu for mobile browsers
    var siteMenuClone = function () {
        console.log("siteMenuClone");

        $('<div class="site-mobile-menu"></div>').prependTo('.site-wrap');

        $('<div class="site-mobile-menu-header"></div>').prependTo('.site-mobile-menu');
        $('<div class="site-mobile-menu-close "></div>').prependTo('.site-mobile-menu-header');
        $('<div class="site-mobile-menu-logo"></div>').prependTo('.site-mobile-menu-header');

        $('<div class="site-mobile-menu-body"></div>').appendTo('.site-mobile-menu');



        $('.js-logo-clone').clone().appendTo('.site-mobile-menu-logo');

        $('<span class="ion-ios-close js-menu-toggle"></div>').prependTo('.site-mobile-menu-close');


        $('.js-clone-nav').each(function () {
            var $this = $(this);
            console.log("cloning:" + $this)
            $this.clone().attr('class', 'site-nav-wrap').appendTo('.site-mobile-menu-body');
        });


        setTimeout(function () {

            var counter = 0;
            $('.site-mobile-menu .has-children').each(function () {
                var $this = $(this);

                $this.prepend('<span class="arrow-collapse collapsed">');

                $this.find('.arrow-collapse').attr({
                    'data-toggle': 'collapse',
                    'data-target': '#collapseItem' + counter,
                });

                $this.find('> ul').attr({
                    'class': 'collapse',
                    'id': 'collapseItem' + counter,
                });

                counter++;

            });

        }, 1000);

        $('body').on('click', '.arrow-collapse', function (e) {
            var $this = $(this);
            if ($this.closest('li').find('.collapse').hasClass('show')) {
                $this.removeClass('active');
            } else {
                $this.addClass('active');
            }
            e.preventDefault();

        });

        $(window).resize(function () {
            var $this = $(this),
                w = $this.width();

            console.log("width=" + w)
            if (w > 768) {
                if ($('body').hasClass('offcanvas-menu')) {
                    $('body').removeClass('offcanvas-menu');
                }
            }
        })

        $('body').on('click', '.js-menu-toggle', function (e) {
            var $this = $(this);
            e.preventDefault();

            if ($('body').hasClass('offcanvas-menu')) {
                $('body').removeClass('offcanvas-menu');
                $this.removeClass('active');
            } else {
                $('body').addClass('offcanvas-menu');
                $this.addClass('active');
            }
        })

        // click outside offcanvas
        $(document).mouseup(function (e) {
            var container = $(".site-mobile-menu");
            if (!container.is(e.target) && container.has(e.target).length === 0) {
                if ($('body').hasClass('offcanvas-menu')) {
                    $('body').removeClass('offcanvas-menu');
                }
            }
        });
    };
    siteMenuClone();

    // animate out any auto dismiss alerts
    var autoDismiss = function () {
        $(".auto-dismiss").each(function (i, obj) {
            $(this).fadeTo(2000, 500).slideUp(500, function () {
                $(this).slideUp(500);
            });
        });
    }
    autoDismiss();

    var searchShow = function () {
        // alert();
        var searchWrap = $('.search-wrap');
        $('.js-search-open').on('click', function (e) {
            e.preventDefault();
            searchWrap.addClass('active');
            setTimeout(function () {
                searchWrap.find('.form-control').focus();
            }, 300);
        });
        $('.js-search-close').on('click', function (e) {
            e.preventDefault();
            searchWrap.removeClass('active');
        })
    };
    searchShow();

    var slider = function () {
        $('.nonloop-block-3').owlCarousel({
            center: false,
            items: 1,
            loop: true,
            smartSpeed: 700,
            stagePadding: 15,
            margin: 20,
            autoplay: true,
            nav: true,
            navText: ['<span class="icon-arrow_back">', '<span class="icon-arrow_forward">'],
            responsive: {
                600: {
                    margin: 20,
                    items: 2
                },
                1000: {
                    margin: 20,
                    items: 3
                },
                1200: {
                    margin: 20,
                    items: 3
                }
            }
        });
    };
    slider();

    function GetURLParameter(sParam) {
        var sPageURL = window.location.search.substring(1);
        var sURLVariables = sPageURL.split('&');
        for (var i = 0; i < sURLVariables.length; i++) {
            var sParameterName = sURLVariables[i].split('=');
            if (sParameterName[0] == sParam) {
                return sParameterName[1];
            }
        }
    }

});

window.FontAwesomeConfig = {
    searchPseudoElements: true
}
