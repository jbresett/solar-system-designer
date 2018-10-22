mergeInto(LibraryManager.library, {

  

  postMessage: function (str)
  {
    window.parent.postMessage(Pointer_stringify(str), '*');
  },


  bindMessageListener: function (receiverName)
  {
	if(this.message_event_handler != null)
		return;

	var objectName = Pointer_stringify(receiverName);

    this.message_event_handler = function(event)
    {
      window.gameInstance.SendMessage(objectName, 'receiveMessage', event.data);
    }

    window.addEventListener('message', this.message_event_handler);
  },

  isInIFrame: function()
  {
      return window !== window.parent;
  },

  isInAuthor: function()
  {
	return document.referrer.indexOf('/bronte/author') !== -1;
  },

  setKeyPairSessionStorage: function(simId, key, value)
  {
	var javaSimId = Pointer_stringify(simId);
	var javaKey = Pointer_stringify(key);
	var javaValue = Pointer_stringify(value);

	var success = true;
	try
	{
		var simData = JSON.parse(window.sessionStorage.getItem(javaSimId)) || {};
		simData[javaKey] = javaValue;
		window.sessionStorage.setItem(javaSimId, JSON.stringify(simData));
	}
	catch(err){
		success = false;
		console.warn('An error occurred while trying to save the data to sessionStorage.');
	}

	return success;
  },

  getKeyPairSessionStorage: function(simId, key)
  {
	var javaSimId = Pointer_stringify(simId);
	var javaKey = Pointer_stringify(key);

	var response =
	{
		success: true,
		key: javaKey,
		value: null,
		exists: false
	};

    try
	{
        var simData = JSON.parse(window.sessionStorage.getItem(javaSimId));
        if(simData && simData.hasOwnProperty(javaKey)){
            response.value = simData[javaKey];
            response.exists = true;
        }
    }
    catch(err){
        console.warn('An error occurred while reading the date from sessionStorage.');
		response.success = false;
    }

	// Create an allocate buffer for returning string to unity
	// If the string is a return value, the unity documentation states that the il2cpp runtime will take care of freeing the memory
	var returnStr = JSON.stringify(response);
    var bufferSize = lengthBytesUTF8(returnStr) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(returnStr, buffer, bufferSize);
    return buffer;
  },

    getDomain: function()
	{
		var returnStr = document.domain;
		var bufferSize = lengthBytesUTF8(returnStr) + 1;
		var buffer = _malloc(bufferSize);
		stringToUTF8(returnStr, buffer, bufferSize);
		return buffer;
    },

	setDomain: function(newDomain)
	{
		document.domain = Pointer_stringify(newDomain);
	},


});