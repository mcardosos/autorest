/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License. See License.txt in the project root for
 * license information.
 * 
 * Code generated by Microsoft (R) AutoRest Code Generator 0.14.0.0
 * Changes may cause incorrect behavior and will be lost if the code is
 * regenerated.
*/

import { ServiceClientOptions, RequestOptions, ServiceCallback } from 'ms-rest';
import * as models from '../models';


/**
 * @class
 * String
 * __NOTE__: An instance of this class is automatically created for an
 * instance of the AutoRestSwaggerBATService.
 */
export interface String {

    /**
     * Get null string value value
     *
     * @param {object} [options] Optional Parameters.
     * 
     * @param {object} [options.customHeaders] Headers that will be added to the
     * request
     * 
     * @param {ServiceCallback} [callback] callback function; see ServiceCallback
     * doc in ms-rest index.d.ts for details
     */
    getNull(options: { customHeaders? : { [headerName: string]: string; } }, callback: ServiceCallback<string>): void;
    getNull(callback: ServiceCallback<string>): void;

    /**
     * Set string value null
     *
     * @param {object} [options] Optional Parameters.
     * 
     * @param {string} [options.stringBody] Possible values for this parameter
     * include: ''
     * 
     * @param {object} [options.customHeaders] Headers that will be added to the
     * request
     * 
     * @param {ServiceCallback} [callback] callback function; see ServiceCallback
     * doc in ms-rest index.d.ts for details
     */
    putNull(options: { stringBody? : string, customHeaders? : { [headerName: string]: string; } }, callback: ServiceCallback<void>): void;
    putNull(callback: ServiceCallback<void>): void;

    /**
     * Get empty string value value ''
     *
     * @param {object} [options] Optional Parameters.
     * 
     * @param {object} [options.customHeaders] Headers that will be added to the
     * request
     * 
     * @param {ServiceCallback} [callback] callback function; see ServiceCallback
     * doc in ms-rest index.d.ts for details
     */
    getEmpty(options: { customHeaders? : { [headerName: string]: string; } }, callback: ServiceCallback<string>): void;
    getEmpty(callback: ServiceCallback<string>): void;

    /**
     * Set string value empty ''
     *
     * @param {string} stringBody Possible values for this parameter include: ''
     * 
     * @param {object} [options] Optional Parameters.
     * 
     * @param {object} [options.customHeaders] Headers that will be added to the
     * request
     * 
     * @param {ServiceCallback} [callback] callback function; see ServiceCallback
     * doc in ms-rest index.d.ts for details
     */
    putEmpty(stringBody: string, options: { customHeaders? : { [headerName: string]: string; } }, callback: ServiceCallback<void>): void;
    putEmpty(stringBody: string, callback: ServiceCallback<void>): void;

    /**
     * Get mbcs string value
     * '啊齄丂狛狜隣郎隣兀﨩ˊ▇█〞〡￤℡㈱‐ー﹡﹢﹫、〓ⅰⅹ⒈€㈠㈩ⅠⅫ！￣ぁんァヶΑ︴АЯаяāɡㄅㄩ─╋︵﹄︻︱︳︴ⅰⅹɑɡ〇〾⿻⺁䜣€ '
     *
     * @param {object} [options] Optional Parameters.
     * 
     * @param {object} [options.customHeaders] Headers that will be added to the
     * request
     * 
     * @param {ServiceCallback} [callback] callback function; see ServiceCallback
     * doc in ms-rest index.d.ts for details
     */
    getMbcs(options: { customHeaders? : { [headerName: string]: string; } }, callback: ServiceCallback<string>): void;
    getMbcs(callback: ServiceCallback<string>): void;

    /**
     * Set string value mbcs
     * '啊齄丂狛狜隣郎隣兀﨩ˊ▇█〞〡￤℡㈱‐ー﹡﹢﹫、〓ⅰⅹ⒈€㈠㈩ⅠⅫ！￣ぁんァヶΑ︴АЯаяāɡㄅㄩ─╋︵﹄︻︱︳︴ⅰⅹɑɡ〇〾⿻⺁䜣€ '
     *
     * @param {string} stringBody Possible values for this parameter include:
     * '啊齄丂狛狜隣郎隣兀﨩ˊ▇█〞〡￤℡㈱‐ー﹡﹢﹫、〓ⅰⅹ⒈€㈠㈩ⅠⅫ！￣ぁんァヶΑ︴АЯаяāɡㄅㄩ─╋︵﹄︻︱︳︴ⅰⅹɑɡ〇〾⿻⺁䜣€ '
     * 
     * @param {object} [options] Optional Parameters.
     * 
     * @param {object} [options.customHeaders] Headers that will be added to the
     * request
     * 
     * @param {ServiceCallback} [callback] callback function; see ServiceCallback
     * doc in ms-rest index.d.ts for details
     */
    putMbcs(stringBody: string, options: { customHeaders? : { [headerName: string]: string; } }, callback: ServiceCallback<void>): void;
    putMbcs(stringBody: string, callback: ServiceCallback<void>): void;

    /**
     * Get string value with leading and trailing whitespace
     * '<tab><space><space>Now is the time for all good men to come to the aid of
     * their country<tab><space><space>'
     *
     * @param {object} [options] Optional Parameters.
     * 
     * @param {object} [options.customHeaders] Headers that will be added to the
     * request
     * 
     * @param {ServiceCallback} [callback] callback function; see ServiceCallback
     * doc in ms-rest index.d.ts for details
     */
    getWhitespace(options: { customHeaders? : { [headerName: string]: string; } }, callback: ServiceCallback<string>): void;
    getWhitespace(callback: ServiceCallback<string>): void;

    /**
     * Set String value with leading and trailing whitespace
     * '<tab><space><space>Now is the time for all good men to come to the aid of
     * their country<tab><space><space>'
     *
     * @param {string} stringBody Possible values for this parameter include: '
     * Now is the time for all good men to come to the aid of their country
     * '
     * 
     * @param {object} [options] Optional Parameters.
     * 
     * @param {object} [options.customHeaders] Headers that will be added to the
     * request
     * 
     * @param {ServiceCallback} [callback] callback function; see ServiceCallback
     * doc in ms-rest index.d.ts for details
     */
    putWhitespace(stringBody: string, options: { customHeaders? : { [headerName: string]: string; } }, callback: ServiceCallback<void>): void;
    putWhitespace(stringBody: string, callback: ServiceCallback<void>): void;

    /**
     * Get String value when no string value is sent in response payload
     *
     * @param {object} [options] Optional Parameters.
     * 
     * @param {object} [options.customHeaders] Headers that will be added to the
     * request
     * 
     * @param {ServiceCallback} [callback] callback function; see ServiceCallback
     * doc in ms-rest index.d.ts for details
     */
    getNotProvided(options: { customHeaders? : { [headerName: string]: string; } }, callback: ServiceCallback<string>): void;
    getNotProvided(callback: ServiceCallback<string>): void;
}

/**
 * @class
 * EnumModel
 * __NOTE__: An instance of this class is automatically created for an
 * instance of the AutoRestSwaggerBATService.
 */
export interface EnumModel {

    /**
     * Get enum value 'red color' from enumeration of 'red color', 'green-color',
     * 'blue_color'.
     *
     * @param {object} [options] Optional Parameters.
     * 
     * @param {object} [options.customHeaders] Headers that will be added to the
     * request
     * 
     * @param {ServiceCallback} [callback] callback function; see ServiceCallback
     * doc in ms-rest index.d.ts for details
     */
    getNotExpandable(options: { customHeaders? : { [headerName: string]: string; } }, callback: ServiceCallback<string>): void;
    getNotExpandable(callback: ServiceCallback<string>): void;

    /**
     * Sends value 'red color' from enumeration of 'red color', 'green-color',
     * 'blue_color'
     *
     * @param {string} stringBody Possible values for this parameter include: 'red
     * color', 'green-color', 'blue_color'
     * 
     * @param {object} [options] Optional Parameters.
     * 
     * @param {object} [options.customHeaders] Headers that will be added to the
     * request
     * 
     * @param {ServiceCallback} [callback] callback function; see ServiceCallback
     * doc in ms-rest index.d.ts for details
     */
    putNotExpandable(stringBody: string, options: { customHeaders? : { [headerName: string]: string; } }, callback: ServiceCallback<void>): void;
    putNotExpandable(stringBody: string, callback: ServiceCallback<void>): void;
}