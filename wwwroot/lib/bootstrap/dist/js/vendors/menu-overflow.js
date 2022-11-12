;(function ( $, window, document, undefined ) {
	$.fn.menuOverflow = function( options ) {
        var ul        = $( this ),
        	parent    = options.parent ? options.parent : ul.parents( '.navbar' ),
        	collapsed = $( '.nav-toggle' ).is( ':visible' ),
        	dropdown  = $( 'li.overflow-nav', ul ),
        	width     = 0;

        // Only process dropdowns if not collapsed
        if ( collapsed === false ) {
            // Get width of the navbar parent so we know how much room we have to work with
            var parent_width = $( parent ).width() - ( options.offset ? parseInt( $( options.offset ).width() ) : 0 );

            // Create one if none exists
            if (! dropdown.length) {
                dropdown = $( '<li class="overflow-nav dropdown"></li>' );
                dropdown.append( $('<a class="dropdown-toggle" data-toggle="dropdown" href="#">' + options.more + ' <i class="fa fa-caret-down"></i></a>' ) );
                dropdown.append( $('<ul class="dropdown-menu"></ul>' ) );
            }

            // Get the width of the navbar, need to add together <li>s as the ul wraps in bootstrap
            ul.children( 'li' ).each( function() {
                var $this = $( this );
                width    += $this.outerWidth();
            });

            // Window is shrinking
            if ( width >= parent_width ) {
                // Loop through each non-dropdown li in the ul menu from right to left (using .get().reverse())
	            $( $( 'li', ul ).not( '.dropdown li' ).get().reverse()).each( function() {

                     // Allow for padding in navbar items
                    var width = 100;

                    // Get the width of the navbar
                    ul.children( 'li' ).each( function() {
                        var $this = $(this);
                        width += $this.outerWidth();
                    });

                    if ( width >= parent_width ) {
                        // Remember the original width so that we can restore as the window grows
                        $( this ).attr( 'data-original-width', $( this ).outerWidth() );

                        // Move the rightmost item to top of dropdown menu if we are running out of space
                        dropdown.children( 'ul.dropdown-menu' ).prepend( this );

						// Make sure the moved menu item doesn't display dropdown
						$( this ).find( 'ul.dropdown-menu' ).addClass( 'hidden' );
                    }
                });
            }
            // Window is growing
            else {
                dropdown.children( 'ul.dropdown-menu' ).children().each( function() {
	                // Restore the topmost dropdown item to the main menu
                    if ( width + parseInt( $( this ).attr( 'data-original-width' ) ) < parent_width ) {
                        dropdown.before( this );

                        // Make sure the moved menu item doesn't display dropdown
						$( this ).find( 'ul.dropdown-menu' ).removeClass( 'hidden' );
                    }
                    // If the topmost item can't be restored, don't look any further
                    else {
                        return false;
                    }
                });
            }

            // Remove or add dropdown depending on whether or not it contains menu items
            if ( !dropdown.children( 'ul.dropdown-menu' ).children().length ) {
                dropdown.remove();
            }
            else {
                // Append new dropdown menu to main menu if it doesn't already exist
                if ( !ul.children( 'li.overflow-nav' ).length ) {
                    ul.append(dropdown);
                }
            }
        }
        else {
			if ( dropdown.length ) {
				// Restore dropdown items to the main menu
				dropdown.children( 'ul.dropdown-menu' ).children().each( function() {
					dropdown.before( this );
				});

				// Remove the dropdown menu
				dropdown.remove();
			}
		}
	};

})( jQuery, window, document );