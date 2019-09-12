import * as $ from 'jquery';

export class Helpers {
    static loadStyles(tag, src) {
        if (Array.isArray(src)) {
            // tslint:disable-next-line:only-arrow-functions
            $.each(src, function(k, s) {
                $(tag).append($('<link/>').attr('href', s).attr('rel', 'stylesheet').attr('type', 'text/css'));
            });
        } else {
            $(tag).append($('<link/>').attr('href', src).attr('rel', 'stylesheet').attr('type', 'text/css'));
        }
    }

    static unwrapTag(element) {
        $(element).removeAttr('appunwraptag').unwrap();
    }

    static setTitle(title) {
        $('.m-subheader__title').text(title);
    }

    static setBreadcrumbs(breadcrumbs) {
        if (breadcrumbs) { $('.m-subheader__title').addClass('m-subheader__title--separator'); }

        let ul = $('.m-subheader__breadcrumbs');

        if ($(ul).length === 0) {
            ul = $('<ul/>').addClass('m-subheader__breadcrumbs m-nav m-nav--inline')
                .append($('<li/>').addClass('m-nav__item')
                    .append($('<a/>').addClass('m-nav__link m-nav__link--icon')
                        .append($('<i/>').addClass('m-nav__link-icon la la-home'))));
        }

        $(ul).find('li:not(:first-child)').remove();
        // tslint:disable-next-line:only-arrow-functions
        $.each(breadcrumbs, function(k, v) {
            const li = $('<li/>').addClass('m-nav__item')
                .append($('<a/>').addClass('m-nav__link m-nav__link--icon').attr('routerLink', v.href).attr('title', v.title)
                    .append($('<span/>').addClass('m-nav__link-text').text(v.text)));
            $(ul).append($('<li/>').addClass('m-nav__separator').text('-')).append(li);
        });
        $('.m-subheader .m-stack__item:first-child').append(ul);
    }

    static setLoading(enable) {
        const body = $('body');
        if (enable) {
            $(body).addClass('m-page--loading-non-block');
            $('.body-blockui').addClass('active');
        } else {
            $(body).removeClass('m-page--loading-non-block');
            $('.body-blockui').removeClass('active');
        }
    }

    static bodyClass(strClass) {
        $('body').attr('class', strClass);
    }

    static addClass(strClass, element) {
        $('#' + element).attr('class', strClass);
    }
}
