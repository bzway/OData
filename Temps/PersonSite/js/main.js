
/*********************
 **********************
 *** Main.js        ***
 **********************
 **********************/
//Note: Ignored Warnings for this, messy white spaces and bitwise operators while validating the script
//Use strict mode to ensure the variable declaration before use
"use strict";



/*** Navigation Button Action Script ***/
/**************************************/

//Initializing the selectors in jquery objects
var menu;
var $menuBtn;
var $homeSection ;
var $aboutSection;
var $contactSection;
var $testSection;
var $overlay;
var $overlayAdhrit;
var $navToggle;
var $preLoader;
var timeScrollTopMs;


//Selector for menu buttons
$menuBtn = $(".menu-btn");
//Selector for About section
$aboutSection = $(".about-sec");
//Selector for Home section
$homeSection = $(".home-sec");
//Selector for Contact Us section
$contactSection =$(".contact-sec");
//Selector for Contact Us section
$testSection =$(".test-sec");
//Selector for Overlay section
$overlay = $(".overlay");
//Selector for navigation section
$navToggle = $(".nav-toggle");
//Selector for Overlay me section
$overlayAdhrit = $(".overlay-me");
//Selector for Se Pre section
$preLoader = $(".main-loader");

timeScrollTopMs = 1500;

//Click Event Handler for the Menu Click Button
function menuBtnClickHandler()
{
    menu = $(this).data("menu");
    switch(menu){
        case 1:
            if($aboutSection.is(":visible")){
                $(this).removeClass("nav-btn");
                $(this).removeClass("focus");
                $homeSection.show();
                $aboutSection.hide();
                $contactSection.hide();
                $testSection.hide();
            }else if(!$aboutSection.is(":visible")){
                $(this).addClass("nav-btn");
                $(this).addClass("focus");
                $aboutSection.show();
                $contactSection.hide();
                $homeSection.hide();
                $testSection.hide();
                $overlay.hide();
                $('html, body').animate({
                    scrollTop: $aboutSection.offset().top
                }, timeScrollTopMs);
            }
            break;

        case 2:
            if($contactSection.is(":visible")){
                $(this).removeClass("nav-btn");
                $(this).removeClass("focus");
                $homeSection.show();
                $aboutSection.hide();
                $contactSection.hide();
                $testSection.hide();
            }else if(!$contactSection.is(":visible")){
                $(this).addClass("nav-btn");
                $(this).addClass("focus");
                $contactSection.show();
                $testSection.hide();
                $aboutSection.hide();
                $homeSection.hide();
                $overlay.hide();
                $('html, body').animate({
                    scrollTop: $contactSection.offset().top
                }, timeScrollTopMs);
            }
            break;
        case 3:

            if($testSection.is(":visible")){
                $(this).removeClass("nav-btn");
                $(this).removeClass("focus");
                $homeSection.show();
                $aboutSection.hide();
                $contactSection.hide();
                $testSection.hide();
            }else if(!$testSection.is(":visible")){
                $(this).addClass("nav-btn");
                $(this).addClass("focus");
                $contactSection.hide();
                $aboutSection.hide();
                $homeSection.hide();
                $testSection.show();
                $overlay.hide();
                $('html, body').animate({
                    scrollTop: $testSection.offset().top
                }, timeScrollTopMs);
            }
            break;
    }
}


function navBtnClickHandler(){
    $(this).toggleClass("active");
    $overlayAdhrit.toggleClass("open");
    $overlayAdhrit.show();
    if($homeSection.is(":visible")){
        $(this).removeClass("nav-btn");
        $(this).removeClass("focus");
        $homeSection.show();
        $aboutSection.hide();
        $contactSection.hide();
        $testSection.hide();
    }else if(!$homeSection.is(":visible")){
        $(this).addClass("nav-btn");
        $(this).addClass("focus");
        $homeSection.show();
        $aboutSection.hide();
        $contactSection.hide();
        $testSection.hide();
        $overlay.hide();
    }
}

//Bind Menu button click event
$menuBtn.on('click',menuBtnClickHandler);

//Bind Navigation Button click event
$navToggle.on('click',navBtnClickHandler);

/*** Navigation Button Action Script END ***/
/******************************************/

/*** Overlay Nav Button Action Script ***/
/***************************************/

//Initializing the selectors in jquery objects
var $;
var $menu;
var $navToggle;
var $overlayLeft;

//Selector for Nav buttons
$menu = $('.menu');
$navToggle = $('.nav-toggle');
$overlayLeft = $('.overlay-left');

function toggleMenu(){
    $menu.click(function(){
        $overlayLeft.fadeIn('fast');
    });
    $navToggle.click(function(){
        $overlayLeft.fadeOut('fast');
    });
}
toggleMenu();

/*** Overlay Nav Button Action Script END ***/
/********************************************/

/*** Preloader Script ***/
/***********************/

$(window).load(function() {
    $preLoader.fadeOut("slow"); // Animate loader off screen
});

/*** Preloader Script END ***/
/***************************/


/*** Text rotation ***/
/***********************/

jQuery(document).ready(function($){
    //set animation timing
    var animationDelay = 2500,
    //loading bar effect
        barAnimationDelay = 3800,
        barWaiting = barAnimationDelay - 3000, //3000 is the duration of the transition on the loading bar - set in the scss/css file
    //letters effect
        lettersDelay = 50,
    //type effect
        typeLettersDelay = 150,
        selectionDuration = 500,
        typeAnimationDelay = selectionDuration + 800,
    //clip effect
        revealDuration = 600,
        revealAnimationDelay = 1500;

    initHeadline();


    function initHeadline() {
        //insert <i> element for each letter of a changing word
        singleLetters($('.cd-headline.letters').find('b'));
        //initialise headline animation
        animateHeadline($('.cd-headline'));
    }

    function singleLetters($words) {
        $words.each(function(){
            var word = $(this),
                letters = word.text().split(''),
                selected = word.hasClass('is-visible');
            for (i in letters) {
                if(word.parents('.rotate-2').length > 0) letters[i] = '<em>' + letters[i] + '</em>';
                letters[i] = (selected) ? '<i class="in">' + letters[i] + '</i>': '<i>' + letters[i] + '</i>';
            }
            var newLetters = letters.join('');
            word.html(newLetters).css('opacity', 1);
        });
    }

    function animateHeadline($headlines) {
        var duration = animationDelay;
        $headlines.each(function(){
            var headline = $(this);

            if(headline.hasClass('loading-bar')) {
                duration = barAnimationDelay;
                setTimeout(function(){ headline.find('.cd-words-wrapper').addClass('is-loading') }, barWaiting);
            } else if (headline.hasClass('clip')){
                var spanWrapper = headline.find('.cd-words-wrapper'),
                    newWidth = spanWrapper.width() + 10
                spanWrapper.css('width', newWidth);
            } else if (!headline.hasClass('type') ) {
                //assign to .cd-words-wrapper the width of its longest word
                var words = headline.find('.cd-words-wrapper b'),
                    width = 0;
                words.each(function(){
                    var wordWidth = $(this).width();
                    if (wordWidth > width) width = wordWidth;
                });
                headline.find('.cd-words-wrapper').css('width', width);
            };

            //trigger animation
            setTimeout(function(){ hideWord( headline.find('.is-visible').eq(0) ) }, duration);
        });
    }

    function hideWord($word) {
        var nextWord = takeNext($word);

        if($word.parents('.cd-headline').hasClass('type')) {
            var parentSpan = $word.parent('.cd-words-wrapper');
            parentSpan.addClass('selected').removeClass('waiting');
            setTimeout(function(){
                parentSpan.removeClass('selected');
                $word.removeClass('is-visible').addClass('is-hidden').children('i').removeClass('in').addClass('out');
            }, selectionDuration);
            setTimeout(function(){ showWord(nextWord, typeLettersDelay) }, typeAnimationDelay);

        } else if($word.parents('.cd-headline').hasClass('letters')) {
            var bool = ($word.children('i').length >= nextWord.children('i').length) ? true : false;
            hideLetter($word.find('i').eq(0), $word, bool, lettersDelay);
            showLetter(nextWord.find('i').eq(0), nextWord, bool, lettersDelay);

        }  else if($word.parents('.cd-headline').hasClass('clip')) {
            $word.parents('.cd-words-wrapper').animate({ width : '2px' }, revealDuration, function(){
                switchWord($word, nextWord);
                showWord(nextWord);
            });

        } else if ($word.parents('.cd-headline').hasClass('loading-bar')){
            $word.parents('.cd-words-wrapper').removeClass('is-loading');
            switchWord($word, nextWord);
            setTimeout(function(){ hideWord(nextWord) }, barAnimationDelay);
            setTimeout(function(){ $word.parents('.cd-words-wrapper').addClass('is-loading') }, barWaiting);

        } else {
            switchWord($word, nextWord);
            setTimeout(function(){ hideWord(nextWord) }, animationDelay);
        }
    }

    function showWord($word, $duration) {
        if($word.parents('.cd-headline').hasClass('type')) {
            showLetter($word.find('i').eq(0), $word, false, $duration);
            $word.addClass('is-visible').removeClass('is-hidden');

        }  else if($word.parents('.cd-headline').hasClass('clip')) {
            $word.parents('.cd-words-wrapper').animate({ 'width' : $word.width() + 10 }, revealDuration, function(){
                setTimeout(function(){ hideWord($word) }, revealAnimationDelay);
            });
        }
    }

    function hideLetter($letter, $word, $bool, $duration) {
        $letter.removeClass('in').addClass('out');

        if(!$letter.is(':last-child')) {
            setTimeout(function(){ hideLetter($letter.next(), $word, $bool, $duration); }, $duration);
        } else if($bool) {
            setTimeout(function(){ hideWord(takeNext($word)) }, animationDelay);
        }

        if($letter.is(':last-child') && $('html').hasClass('no-csstransitions')) {
            var nextWord = takeNext($word);
            switchWord($word, nextWord);
        }
    }

    function showLetter($letter, $word, $bool, $duration) {
        $letter.addClass('in').removeClass('out');

        if(!$letter.is(':last-child')) {
            setTimeout(function(){ showLetter($letter.next(), $word, $bool, $duration); }, $duration);
        } else {
            if($word.parents('.cd-headline').hasClass('type')) { setTimeout(function(){ $word.parents('.cd-words-wrapper').addClass('waiting'); }, 200);}
            if(!$bool) { setTimeout(function(){ hideWord($word) }, animationDelay) }
        }
    }

    function takeNext($word) {
        return (!$word.is(':last-child')) ? $word.next() : $word.parent().children().eq(0);
    }

    function takePrev($word) {
        return (!$word.is(':first-child')) ? $word.prev() : $word.parent().children().last();
    }

    function switchWord($oldWord, $newWord) {
        $oldWord.removeClass('is-visible').addClass('is-hidden');
        $newWord.removeClass('is-hidden').addClass('is-visible');
    }
});

