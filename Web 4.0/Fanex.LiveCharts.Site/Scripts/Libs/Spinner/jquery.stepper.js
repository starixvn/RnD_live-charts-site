/*
 * Stepper Plugin [Formstone Library]
 * @author Ben Plum
 * @version 0.1.3
 *
 * Copyright © 2012 Ben Plum <mr@benplum.com>
 * Released under the MIT License <http://www.opensource.org/licenses/mit-license.php>
 */
 
if (jQuery) (function($) {
	
	// Mobile Detect
	var agent = isMobile = /Android|webOS|iPhone|iPad|iPod|BlackBerry/i.test( (navigator.userAgent||navigator.vendor||window.opera) );
	
	// Default Options
	var options = {
		customClass: ""
	};
	
	var timer = null;
	
	// Public Methods
	var pub = {
		
		// Set Defaults
		defaults: function(opts) {
			options = $.extend(options, opts || {});
			return $(this);
		},
		
		// Disable field
		disable: function() {
			return $(this).each(function(i) {
				var $input = $(this),
					$stepper = $input.parent(".stepper");
				
				$input.attr("disabled", "disabled");
				$stepper.addClass("disabled");
			});
		},
		
		// Enable field
		enable: function() {
			return $(this).each(function(i) {
				var $input = $(this),
					$stepper = $input.parent(".stepper");
				
				$input.attr("disabled", null);
				$stepper.removeClass("disabled");
			});
		},
		
		// Destroy stepper
		destroy: function() {
			return $(this).each(function(i) {
				var $input = $(this),
					$stepper = $input.parent(".stepper");
				
				// Unbind click events
				$stepper.off(".stepper")
						.find(".stepper-step")
						.remove();
				
				// Restore DOM
				$input.unwrap()
					  .removeClass("stepper-input");
			});
		}
	};
	
	// Private Methods
	
	// Initialize
	function _init(opts) {
		opts = opts || {};
		// Check for mobile
		if (isMobile) {
			opts.trueMobile = true;
			if (typeof opts.mobile === "undefined") {
				opts.mobile = true;
			}
		}
		
		// Define settings
		var settings = $.extend({}, options, opts);
		
		// Apply to each element
		return $(this).each(function(i) {
			var $input = $(this);
			
			if (!$input.data("stepper")) {
				var min = parseFloat($input.attr("min")),
					max = parseFloat($input.attr("max")),
					step = parseFloat($input.attr("step")) || 1;
				
				// Modify DOM
				$input.addClass("stepper-input")
					  .wrap('<div class="stepper ' + settings.customClass + '" />')
					  .after('<span class="stepper-step up">Up</span><span class="stepper-step down">Down</span>');
				
				// Store plugin data
				var $stepper = $input.parent(".stepper");
				
				// Check disabled
				if ($input.is(":disabled")) {
					$stepper.addClass("disabled");
				}
				
				var data = $.extend({
					$stepper: $stepper,
					$input: $input,
					$steps: $stepper.find(".stepper-steps"),
					min: (typeof min !== undefined && !isNaN(min)) ? min : false,
					max: (typeof max !== undefined && !isNaN(max)) ? max : false,
					step: (typeof step !== undefined && !isNaN(step)) ? step : 1
				}, settings);
				
				data.digits = _significantDigits(data.step);
				
				// Bind click events
				$stepper.on("mousedown.stepper", ".stepper-step", data, _stepDown)
						.data("stepper", data);
						/* .on("keyup.stepper", ".stepper-input", data, _keyUp) */
				// Bind key up and down Event
				$stepper.unbind('keydown').keydown(data, _keyDown);
				$stepper.unbind('mousewheel').bind('mousewheel', data, _mouseWheel);
				$stepper.unbind('keyup').keydown(data, _keyUp);
			}
		});
	}
	
	function _mouseWheel(e)
	{
		if(e.originalEvent.wheelDelta /120 > 0) {
           _doStep(e.data, true, false);
        }
        else{
            _doStep(e.data, false, false);
        }
	};
	
	function _keyUp(e){
		var charCode = e.keyCode;
		
		if(charCode != 39 && charCode != 37)
		{
			var setTimeOutId = setTimeout(function(){
				_constrain(e.data);
				
				clearTimeout(setTimeOutId);
			}, 10);
		}
	};
	
	function _keyDown(e)
	{
		if (e.keyCode == 38) 
        {
			// Up
			_doStep(e.data, true, false);
		}
		if (e.keyCode == 40) 
		{
			// Up down
			_doStep(e.data, false, false);
		}
		
		var charCode = e.keyCode;
		
		// allow special key presses
        if (charCode == 0 || charCode == 8 || charCode == 39 || charCode == 37  || e.altKey || e.ctrlKey || e.metaKey) return;

		// allow digits
        if (charCode >= 48 && charCode <= 57) return;

        if (charCode >= 96 && charCode <= 105) return;

        // prevent the default action
        if (e.preventDefault) {
            e.preventDefault();
        } else {
            window.event.returnValue = false;
        }
	};
	
	// Handle mouse down
	function _stepDown(e) {
		e.preventDefault();
		e.stopPropagation();
		
		_clearStep();
		
		var data = e.data;
		
		if (!data.$stepper.hasClass("disabled")) {
			var direction = $(e.target).hasClass("up");
			
			_doStep(data, direction, true);
			
			$("body").on("mouseup.stepper", data, _stepUp);
		}
	}
	
	// Handle mouse up
	function _stepUp(e) {
		e.preventDefault();
		e.stopPropagation();
		
		_clearStep();
	}
	
	// Update
	function _doStep(data, direction, firstStep) {
		var originalValue = parseFloat(data.$input.val());
		originalValue = (typeof originalValue !== undefined && !isNaN(originalValue)) ? originalValue : 0;
		var value = originalValue + (direction ? data.step : -data.step);
		
		if (data.min !== false && value < data.min) {
			value = data.min;
		}
		if (data.max !== false && value > data.max) {
			value = data.max;
		}
		
		if (value != originalValue) {
			value = _roundDigits(value, data.digits);
			data.$input.val(value)
					   .trigger("change");
			originalValue = value;
		}
		
		if (firstStep) {
			timer = setInterval(function() {
				_doStep(data, direction, false);
			}, 125);
		}
	}
	
	function _constrain(data)
	{
		var value = parseFloat(data.$input.val());
		value = (typeof value !== undefined && !isNaN(value)) ? value : 0;
		
		if (data.min !== false && value < data.min) {
			value = data.min;
		}
		if (data.max !== false && value > data.max) {
			value = data.max;
		}
		data.$input.val(value);
	}
	
	// Clear timeout
	function _clearStep() {
		if (typeof timer !== undefined) {
			clearInterval(timer);
			timer = null;
		}
		
		$("body").off(".stepper");
	}
	
	// Count significant digits
	function _significantDigits(value) {
		var test = new String(value);
		if (test.indexOf(".") > -1) {
			return test.length - test.indexOf(".") - 1;
		} else {
			return 0;
		}
	}
	
	// Round significant digits
	function _roundDigits(value, digits) {
		var exp = Math.pow(10, digits);
		return Math.round(value * exp) / exp;
	}
	
	// Define Plugin
	$.fn.stepper = function(method) {
		if (pub[method]) {
			return pub[method].apply(this, Array.prototype.slice.call(arguments, 1));
		} else if (typeof method === 'object' || !method) {
			return _init.apply(this, arguments);
		}
		return this;
	};
})(jQuery);