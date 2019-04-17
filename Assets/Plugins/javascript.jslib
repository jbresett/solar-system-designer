mergeInto(LibraryManager.library, 
{

	JSAlert: function (message)
	{
		window.alert(Pointer_stringify(message));
	},

	JSConfirm: function (message)
	{
		return window.confirm(Pointer_strify(message));
	},

	JSCanvasWidth: function () 
	{
		return gameInstance.Module.canvas.style.width;
	},

	JSCanvasHeight: function () 
	{
		return gameInstance.Module.canvas.style.height;
	},

	JSPrompt: function (text, value)
	{
		return window.prompt(Pointer_stringify(text), Pointer_stringify(value));
	},
});