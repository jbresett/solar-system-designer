mergeInto(LibraryManager.library, 
{

	JSAlert: function (message)
	{
		window.alert(Pointer_stringify(message));
	},

	JSConfirm: function (message)
	{
		return window.confirm(Pointer_stringify(message));
	},

	JSCanvasWidth: function () 
	{
		return gameInstance.Module.canvas.style.width;
	},

	JSCanvasHeight: function () 
	{
		return gameInstance.Module.canvas.style.height;
	},

	JSPrompt: function (message, defaultText)
	{
		var result = window.prompt(Pointer_stringify(message), Pointer_stringify(defaultText));
		if (result === null) return null;
		var bufferSize = lengthBytesUTF8(result) + 1;
		var buffer = gameInstance.Module._malloc(bufferSize);
		stringToUTF8(result, buffer, bufferSize);
		return buffer;
	},

});