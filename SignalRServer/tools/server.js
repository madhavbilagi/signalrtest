/*!
 * ASP.NET SignalR JavaScript Library v2.2.0
 * http://signalr.net/
 *
 * Copyright Microsoft Open Technologies, Inc. All rights reserved.
 * Licensed under the Apache 2.0
 * https://github.com/SignalR/SignalR/blob/master/LICENSE.md
 *
 */

/// <reference path="..\..\SignalR.Client.JS\Scripts\jquery-1.6.4.js" />
/// <reference path="jquery.signalR.js" />
(function ($, window, undefined) {
    /// <param name="$" type="jQuery" />
    "use strict";

    if (typeof ($.signalR) !== "function") {
        throw new Error("SignalR: SignalR is not loaded. Please ensure jquery.signalR-x.js is referenced before ~/signalr/js.");
    }

    var signalR = $.signalR;

    function makeProxyCallback(hub, callback) {
        return function () {
            // Call the client hub method
            callback.apply(hub, $.makeArray(arguments));
        };
    }

    function registerHubProxies(instance, shouldSubscribe) {
        var key, hub, memberKey, memberValue, subscriptionMethod;

        for (key in instance) {
            if (instance.hasOwnProperty(key)) {
                hub = instance[key];

                if (!(hub.hubName)) {
                    // Not a client hub
                    continue;
                }

                if (shouldSubscribe) {
                    // We want to subscribe to the hub events
                    subscriptionMethod = hub.on;
                } else {
                    // We want to unsubscribe from the hub events
                    subscriptionMethod = hub.off;
                }

                // Loop through all members on the hub and find client hub functions to subscribe/unsubscribe
                for (memberKey in hub.client) {
                    if (hub.client.hasOwnProperty(memberKey)) {
                        memberValue = hub.client[memberKey];

                        if (!$.isFunction(memberValue)) {
                            // Not a client hub function
                            continue;
                        }

                        subscriptionMethod.call(hub, memberKey, makeProxyCallback(hub, memberValue));
                    }
                }
            }
        }
    }

    $.hubConnection.prototype.createHubProxies = function () {
        var proxies = {};
        this.starting(function () {
            // Register the hub proxies as subscribed
            // (instance, shouldSubscribe)
            registerHubProxies(proxies, true);

            this._registerSubscribedHubs();
        }).disconnected(function () {
            // Unsubscribe all hub proxies when we "disconnect".  This is to ensure that we do not re-add functional call backs.
            // (instance, shouldSubscribe)
            registerHubProxies(proxies, false);
        });

        proxies['IMIchatHub'] = this.createHubProxy('IMIchatHub'); 
        proxies['IMIchatHub'].client = { };
        proxies['IMIchatHub'].server = {
            boardcastToAgent: function (toUserId, message) {
            /// <summary>Calls the BoardcastToAgent method on the server-side IMIchatHub hub.&#10;Returns a jQuery.Deferred() promise.</summary>
            /// <param name=\"toUserId\" type=\"String\">Server side type is System.String</param>
            /// <param name=\"message\" type=\"String\">Server side type is System.String</param>
                return proxies['IMIchatHub'].invoke.apply(proxies['IMIchatHub'], $.merge(["BoardcastToAgent"], $.makeArray(arguments)));
             },

            boardcastToTeam: function (teamId, message) {
            /// <summary>Calls the BoardcastToTeam method on the server-side IMIchatHub hub.&#10;Returns a jQuery.Deferred() promise.</summary>
            /// <param name=\"teamId\" type=\"String\">Server side type is System.String</param>
            /// <param name=\"message\" type=\"String\">Server side type is System.String</param>
                return proxies['IMIchatHub'].invoke.apply(proxies['IMIchatHub'], $.merge(["BoardcastToTeam"], $.makeArray(arguments)));
             },

            connect: function (userId, TeamID) {
            /// <summary>Calls the Connect method on the server-side IMIchatHub hub.&#10;Returns a jQuery.Deferred() promise.</summary>
            /// <param name=\"userId\" type=\"String\">Server side type is System.String</param>
            /// <param name=\"TeamID\" type=\"Number\">Server side type is System.Int32</param>
                return proxies['IMIchatHub'].invoke.apply(proxies['IMIchatHub'], $.merge(["Connect"], $.makeArray(arguments)));
             },

            disConnect: function () {
            /// <summary>Calls the DisConnect method on the server-side IMIchatHub hub.&#10;Returns a jQuery.Deferred() promise.</summary>
                return proxies['IMIchatHub'].invoke.apply(proxies['IMIchatHub'], $.merge(["DisConnect"], $.makeArray(arguments)));
             },

            hello: function () {
            /// <summary>Calls the Hello method on the server-side IMIchatHub hub.&#10;Returns a jQuery.Deferred() promise.</summary>
                return proxies['IMIchatHub'].invoke.apply(proxies['IMIchatHub'], $.merge(["Hello"], $.makeArray(arguments)));
             }
        };

        return proxies;
    };

    signalR.hub = $.hubConnection("/signalr", { useDefaultPath: false });
    $.extend(signalR, signalR.hub.createHubProxies());

}(window.jQuery, window));