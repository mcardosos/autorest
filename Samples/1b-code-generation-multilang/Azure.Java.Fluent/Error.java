/**
 * Code generated by Microsoft (R) AutoRest Code Generator 1.0.1.0
 * Changes may cause incorrect behavior and will be lost if the code is
 * regenerated.
 */

package petstore;

import com.fasterxml.jackson.annotation.JsonProperty;

/**
 * The Error model.
 */
public class Error {
    /**
     * The code property.
     */
    @JsonProperty(value = "code", required = true)
    private int code;

    /**
     * The message property.
     */
    @JsonProperty(value = "message", required = true)
    private String message;

    /**
     * Get the code value.
     *
     * @return the code value
     */
    public int code() {
        return this.code;
    }

    /**
     * Set the code value.
     *
     * @param code the code value to set
     * @return the Error object itself.
     */
    public Error withCode(int code) {
        this.code = code;
        return this;
    }

    /**
     * Get the message value.
     *
     * @return the message value
     */
    public String message() {
        return this.message;
    }

    /**
     * Set the message value.
     *
     * @param message the message value to set
     * @return the Error object itself.
     */
    public Error withMessage(String message) {
        this.message = message;
        return this;
    }

}
