using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimCapi
{

    public enum SimCapiValueType
    {
        NUMBER = 1,
        STRING = 2,
        ARRAY = 3,
        BOOLEAN = 4,
        ENUM = 5,
        MATH_EXPR = 6,
        ARRAY_POINT = 7
    }

    public enum SimCapiMessageType
    {
        HANDSHAKE_REQUEST = 1,
        HANDSHAKE_RESPONSE = 2,
        ON_READY = 3,
        VALUE_CHANGE = 4,
        CONFIG_CHANGE = 5,
        VALUE_CHANGE_REQUEST = 6,
        CHECK_REQUEST = 7,
        CHECK_COMPLETE_RESPONSE = 8,
        GET_DATA_REQUEST = 9,
        GET_DATA_RESPONSE = 10,
        SET_DATA_REQUEST = 11,
        SET_DATA_RESPONSE = 12,
        INITIAL_SETUP_COMPLETE = 14,
        CHECK_START_RESPONSE = 15,
        API_CALL_REQUEST = 16,
        API_CALL_RESPONSE = 17,
        RESIZE_PARENT_CONTAINER_REQUEST = 18,
        RESIZE_PARENT_CONTAINER_RESPONSE = 19,
        ALLOW_INTERNAL_ACCESS = 20,
        REGISTER_LOCAL_DATA_CHANGE_LISTENER = 21,
        REGISTERED_LOCAL_DATA_CHANGED = 22
    }

    public enum ChangedBy
    {
        AELP,
        SIM
    }

}


