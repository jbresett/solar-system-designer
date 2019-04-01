mergeInto(LibraryManager.library, 
{

	JSAlert: function (str)
	{
		window.alert(Pointer_stringify(str));
	},

	JSCanvasWidth: function () 
	{
		return gameInstance.Module.canvas.style.width;
	},

	JSCanvasHeight: function () 
	{
		return gameInstance.Module.canvas.style.height;
	},

});