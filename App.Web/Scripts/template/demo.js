$(document).ready(function(){

	/* Waves */
 	Waves.init();
 	
	/* Sidebar Chart */
	var cssAnimationData = {
		labels: ["S", "M", "T", "W", "T", "F", "S"],
		series: [
			[7, 15, 30, 20, 35, 30, 40]
		]
	},
	cssAnimationOptions = {
		fullWidth: !0,
		chartPadding: {
			right: 21,
			left: -3,
			top: 20,
			bottom: -5
		}
	},
	cssAnimationResponsiveOptions = [
		[{
			axisX: {
				labelInterpolationFnc: function(value, index) {
					return index % 2 !== 0 ? !1 : value
				}
			}
		}]
	];

	new Chartist.Line('#sidebar-chart', cssAnimationData, cssAnimationOptions, cssAnimationResponsiveOptions);

});