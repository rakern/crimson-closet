;(function ( $, window, document, undefined ) {

	var pluginName = "smartEmbed",
	defaults = {
		autoplay: true,
		width: 'auto',
		playIcon: 'fa fa-play-circle-o',
		onComplete: function(){},
		onError: function(){}
	};

	function Plugin( element, options ) {
		this.element   = element;
		this.options   = $.extend( {}, defaults, options );
		this._defaults = defaults;
		this._name     = pluginName;
		this.init();
	}

	Plugin.prototype = {
		init: function() {
			var options = this.options;

			$( this.element ).each( function ( i, e ) {
				var $e         = $( e ),
					vimeoID    = $e.data( 'vimeo-id' ),
					youTubeID  = $e.data( 'youtube-id' ),
					icon       = options.playIcon,
					apiURL     = 'https://player.vimeo.com/video/';

				// Set API URL
				if ( typeof vimeoID !== 'undefined' ) {
					apiURL = 'https://player.vimeo.com/video/' + vimeoID;
				}
				else if ( typeof youTubeID !== 'undefined' ) {
					apiURL = 'https://www.youtube.com/embed/' + youTubeID;
				}

				// Build iframe URL
				var url    = apiURL + '?autoplay=' + options.autoplay,
					iframe = $( '<iframe src="' + url + '" webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe>' );

				if ( options.width === parseInt( options.width, 10 ) ) {
					url += '&width=' + options.width;
				}

				// add play icon and click event listener
				$e.parent().prepend( '<i class="play-icon ' + icon + '"/>' ).on( 'click', function(){
					var $this = $( this );

					// only append video once
					if ( !$this.find( 'iframe' ).length ) {

						// append video iframe and hide poster
						// image and play icon
						$this.append( iframe ).find( 'img, .play-icon' ).hide();
					}

					if ( options.onComplete && typeof( options.onComplete ) === 'function' ) {
						options.onComplete.call( this );
					}
				});
			});
		}
	};

	$.fn[pluginName] = function ( options ) {
		return this.each( function () {
			if ( !$.data(this, "plugin_" + pluginName ) ) {
				$.data( this, "plugin_" + pluginName, new Plugin( this, options ) );
			}
		});
	};

})( jQuery, window, document );